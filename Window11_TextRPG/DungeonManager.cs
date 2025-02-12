using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    internal class DungeonManager : IScene  
    {
        private static DungeonManager? instance;
        public static DungeonManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DungeonManager();
                }
                return instance;
            }
        }
        private DungeonManager() 
        {
            InitMonsterCatches();
            monsters = new List<Monster>();
        }

        List<Monster> monsters;

        public Dictionary<string, int> monsterCatches;

        public void Enter()
        {
            Player enterPlayer = PlayerManager.Instance._Player;     
            int playerHpBeforeEnter = enterPlayer.hp;
            int playerMpBeforeEnter = enterPlayer.mp;
            SetMonsters(enterPlayer);
            while (!enterPlayer.Hpcheck() && GetMonsterDieCount() != monsters.Count)
            {
                EnterDungeon(enterPlayer);
            }
            if (enterPlayer.Hpcheck())                                      //플레이어의 체력이 0일때
            {
                Lose(enterPlayer, playerHpBeforeEnter, playerMpBeforeEnter);
            }
            else if (GetMonsterDieCount() == monsters.Count)                  //몬스터를 전부 처리할 때
            {
                Victory(enterPlayer, playerHpBeforeEnter, playerMpBeforeEnter);
            }
            switch (UtilManager.PlayerInput(0,0))
            {
                case 0:
                    ClearMonsters();
                    GameManager.Instance.ChangeScene(SceneState.LobbyManager);
                    break;
            }
        }

        public void InitMonsterCatches()
        {
            monsterCatches = new Dictionary<string, int>();
            foreach (string monsterTypeName in Enum.GetNames(typeof(MonsterType)))
            {
                monsterCatches.Add(monsterTypeName, 0);
            }
        }

        public void SetMonsters(Player player)
        {
            if (monsters.Count == 0)
            {
                int monsterCount = UtilManager.getRandomInt(1, 5);  //1 ~ 4마리의 몬스터 생성을 위한 값
                for (int i = 0; i < monsterCount; i++)
                {
                    int monsterType = UtilManager.getRandomInt(0, 3);
                    int minLevel = player.stage - 2 <= 0 ? 1 : player.stage - 2;
                    int monsterLevel = UtilManager.getRandomInt(minLevel, player.stage + 3);
                    int monsterHp = UtilManager.getRandomInt(monsterLevel * (monsterType + 5), monsterLevel * (monsterType + 7));
                    monsters.Add(new Monster(monsterLevel, monsterType, monsterHp));
                }
            }
        }

        public void EnterDungeon(Player player)
        {
            int actionInput;
            DisplayManager.DungeonScene(player, monsters);
            actionInput = UtilManager.PlayerInput(0, 2);
            switch (actionInput)
            {
                case 0:
                    ClearMonsters();
                    SetMonsterCatches();
                    GameManager.Instance.ChangeScene(SceneState.LobbyManager);
                    break;
                case 1: 
                    PlayerAttackMonster(player);
                    break;
                case 2:
                    SkillList(player);
                    break;
                default:
                    EnterDungeon(player);
                    break;
            }
        }

        public int TargetMonster(Player player, bool isFail = false)
        {
            int userInput;
            DisplayManager.DungeonMonsterTargetScene(player, monsters, isFail);
            userInput = UtilManager.PlayerInput(0, monsters.Count);
            Monster monster;
            if (userInput == 0)
            {
                return 0;
            }
            else
            {
                monster = monsters[userInput - 1];
                while (monster.IsDie())
                {
                    DisplayManager.DungeonMonsterTargetScene(player, monsters, true);
                    userInput = UtilManager.PlayerInput(0, monsters.Count);
                    if (userInput == 0)
                    {
                        return 0;
                    }
                    monster = monsters[userInput - 1];
                    if (!monster.IsDie())
                    {
                        return userInput;
                    }
                }
            }
            
            return userInput;
        }

        public void PlayerAttackMonster(Player player)
        {
            int selectMonster = TargetMonster(player);
            Monster userSelectedMonster = new Monster();                        //0을 입력시 몬스터 배열의 userInput조회시 -1을 조회시켜 에러발생하여 수정
            if (selectMonster == 0)
            {
                EnterDungeon(player);
            }
            else
            {
                userSelectedMonster = monsters[selectMonster - 1];
                if (!userSelectedMonster.IsDie())
                {
                    int beforeMonsterHp = userSelectedMonster.hp;
                    var (playerDamage, hitType) = player.AttackCalculator(player.Attack()); //playerDamage의 최종 데미지 계산 
                    userSelectedMonster.hp = UtilManager.CalcDamage(userSelectedMonster.hp, playerDamage);
                    DisplayManager.DungeonPlayerAttackScene(player, userSelectedMonster, playerDamage, beforeMonsterHp,hitType);     //플레이어가 몬스터를 공격하는 장면
                    int next = UtilManager.PlayerInput(0, 0);
                    MonsterTurn(player);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    PlayerAttackMonster(player);
                }
            }
        }

        public void MonsterTurn(Player player)
        {
            foreach (Monster monster in monsters)
            {
                if (!monster.IsDie())
                {
                    MonsterAttackPlayer(player, monster);
                }
            }
        }

        public void MonsterAttackPlayer(Player player, Monster monster)
        {
            int beforePlayerHp = player.hp;
            player.hp = UtilManager.CalcDamage(player.hp, monster.Attack());
            if (!player.Hpcheck())
            {
                DisplayManager.DungeonMonsterAttackScene(player, monster, beforePlayerHp);
                int userInput = UtilManager.PlayerInput(0, 0);
            }
        }

        public void Lose(Player player, int playerHpBeforeEnter, int playerMpBeforeEnter)
        {
            SetMonsterCatches();
            DisplayManager.DungeonLoseResultScene(player, playerHpBeforeEnter, playerMpBeforeEnter);
        }

        public void Victory(Player player, int playerHpBeforeEnter, int playerMpBeforeEnter)
        {
            int totalExp = monsters.Sum(monster => monster.level); // 몬스터 레벨 합산하여 경험치 추가

            // 경험치 추가 전 레벨 저장
            int beforeLevel = player.level;
            int expBeforeGain = player.exp;
            player.GainExp(totalExp);
            bool leveledUp = (beforeLevel < player.level);
            int expForNextLevel = player.GetExpForNextLevel();
            Reward reward = InventoryManager.instance.RewardInstnace;
            SetMonsterCatches();
            player.NextStage();
            player.mp = player.mp + 10 > player.maxmp ? player.maxmp : player.mp + 10;
            DisplayManager.DungeonWinResultScene(player, monsters.Count, playerHpBeforeEnter, playerMpBeforeEnter, reward.Gold(), reward.Potion(), reward.Item(), totalExp, expBeforeGain, expForNextLevel, leveledUp);
        }

        private int GetMonsterDieCount()
        {
            int monsterDieCnt = 0;
            foreach (Monster monster in monsters)
            {
                if (monster.IsDie())
                {
                    monsterDieCnt++;
                }
            }

            return monsterDieCnt;
        }

        public void SkillList(Player player)
        {
            int userInput;
            DisplayManager.DungeonSkillList(player, monsters);
            userInput = UtilManager.PlayerInput(0, player.skills.Count());
            if (userInput == 0)
            {
                EnterDungeon(player);
            }
            else
            {
                Skill skill = player.skills[userInput - 1];
                if (!skill.CanUse(player))
                {
                    DisplayManager.CantUseSkill(skill);
                    userInput = UtilManager.PlayerInput(0, 0);
                    SkillList(player);
                }
                switch (skill.type)
                {
                    case 0:
                        TypeOneSKill(player, skill);
                        break;
                    case 1:
                        TypeRandom2Skill(player, skill);
                        break;
                    case 2:
                        TypeAllSkill(player, skill);
                        break;
                }
            }
        }

        public void TypeOneSKill(Player player, Skill skill)
        {
            int userInput = TargetMonster(player);
            Monster monster;
            if(userInput == 0)
            {
                SkillList(player);
            }
            else
            {
                int beforeMonsterHp = 0;
                int skillDamage = 0;
                monster = monsters[userInput - 1];
                beforeMonsterHp = monster.hp;
                if (monster.IsDie())
                {
                    TypeOneSKill(player, skill);
                }
                player.mp -= skill.mp;
                skillDamage = skill.Invoke(player, monsters[userInput - 1]);
                DisplayManager.DungeonSkillOneResult(player, monster, skill, beforeMonsterHp, skillDamage);
                int next = UtilManager.PlayerInput(0, 0);
                MonsterTurn(player);
            }
        }

        public void TypeRandom2Skill(Player player, Skill skill)
        {
            DisplayManager.BeforeRunRandom2(skill);
            int userInput = UtilManager.PlayerInput(0, 1);
            Monster monster;
            if (userInput == 0)
            {
                SkillList(player);
            }
            else
            {
                int[] beforeMonsterHps = new int[2];
                int skillDamage = 0;
                List<Monster> liveMonsters = GetLiveMonsters();
                Monster monster1;
                Monster monster2;

                if (liveMonsters.Count >= 2)
                {
                    monster1 = liveMonsters[UtilManager.getRandomInt(0, liveMonsters.Count)];
                    liveMonsters.Remove(monster1);                      //스킬에 선택된 몬스터는 배열에서 제거하여 중복 방지
                    beforeMonsterHps[0] = monster1.hp;
                    monster2 = liveMonsters[UtilManager.getRandomInt(0, liveMonsters.Count)];
                    beforeMonsterHps[1] = monster2.hp;
                    player.mp -= skill.mp;
                    skillDamage = skill.Invoke(player, monster1);       //스킬 데미지는 동일하여 한번만 값 할당
                    skill.Invoke(player, monster2);
                    DisplayManager.DungeonSkillRandom2Result(player, skill, beforeMonsterHps, skillDamage, monster1, monster2);
                }
                else
                {
                    monster1 = liveMonsters[0];
                    player.mp -= skill.mp;
                    skillDamage = skill.Invoke(player, monster1);
                    DisplayManager.DungeonSkillRandom2Result(player, skill, beforeMonsterHps, skillDamage, monster1);
                }
                
                int next = UtilManager.PlayerInput(0, 0);
                MonsterTurn(player);
            }
        }

        public void TypeAllSkill(Player player, Skill skill)
        {
            int skillDamage = 0;
            List<Monster> liveMonsters = GetLiveMonsters();
            int[] beforeMonsterHps = new int[liveMonsters.Count];
            player.mp -= skill.mp;
            for (int i = 0; i < liveMonsters.Count; i++)
            {
                beforeMonsterHps[i] = liveMonsters[i].hp;
                skillDamage = skill.Invoke(player, liveMonsters[i]);
            }
            DisplayManager.DungeonSkillAllResult(player, skill, beforeMonsterHps, skillDamage, monsters);
            int next = UtilManager.PlayerInput(0, 0);
            MonsterTurn(player);
        }

        public List<Monster> GetLiveMonsters()
        {
            List<Monster> liveMonsters = new List<Monster>();
            foreach (Monster monster in monsters)
            {
                if (!monster.IsDie())
                {
                    liveMonsters.Add(monster);
                }
            }
            return liveMonsters;
        }

        public void SetMonsterCatches()
        {
            foreach (Monster monster in monsters)
            {
                if (monster.IsDie())
                {
                    monsterCatches[Enum.GetName(typeof(MonsterType), monster.type)]++;
                }
            }
        }

        public void ClearMonsters()
        {
            monsters.Clear();
        }

        public enum MonsterType
        {
            Minion = 0,
            Canon,
            VoidMonster
        }

        public enum SkillType
        {
            one = 0,
            random2,
            all
        }
    }
}
