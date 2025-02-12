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
        }

        List<Monster> monsters = new List<Monster>();

        public Dictionary<string,int> monsterCatches = new Dictionary<string, int>();

        public void Enter()
        {
            //Player enterPlayer = new Player("전사", "test", 130, 10);      //테스트용 임시코드 player
            Player enterPlayer = PlayerManager.Instance._Player;     
            int playerHpBeforeEnter = enterPlayer.hp;
            SetMonsters(enterPlayer);
            while (!enterPlayer.Hpcheck() && GetMonsterDieCount() != monsters.Count)
            {
                TargetMonster(enterPlayer);
            }
            if (enterPlayer.Hpcheck())                                      //플레이어의 체력이 0일때
            {
                Lose(enterPlayer, playerHpBeforeEnter);
            }
            else if (GetMonsterDieCount() == monsters.Count)                  //몬스터를 전부 처리할 때
            {
                Victory(enterPlayer, playerHpBeforeEnter);
            }
            switch (UtilManager.PlayerInput(0,0))
            {
                case 0:
                    //상위 메뉴로 이동
                    break;
            }
        }

        public void InitMonsterCatches()
        {
            foreach (string monsterTypeName in Enum.GetNames(typeof(MonsterType)))
            {
                monsterCatches.Add(monsterTypeName, 0);
            }
        }

        public void SetMonsters(Player player)
        {
            int monsterCount = UtilManager.getRandomInt(1,5);  //1 ~ 4마리의 몬스터 생성을 위한 값
            for (int i = 0; i < monsterCount; i++)
            {
                int monsterType = UtilManager.getRandomInt(0, 3);
                int minLevel = player.level - 2 <= 0 ? 1 : player.level - 2;
                int monsterLevel = UtilManager.getRandomInt(minLevel,player.level + 3);
                int monsterHp = UtilManager.getRandomInt(monsterLevel * (monsterType + 5),monsterLevel * (monsterType + 7));
                monsters.Add(new Monster(monsterLevel, monsterType, monsterHp));
            }
        }

        public void TargetMonster(Player player)
        {
            int actionInput;
            DisplayManager.DungeonScene(player, monsters);
            actionInput = UtilManager.PlayerInput(1, 1);
            if (actionInput == 1)
            {
                PlayerAttackMonster(player);
            }else
            {
                TargetMonster(player);
            }
        }

        public void PlayerAttackMonster(Player player)
        {
            int userInput;
            DisplayManager.DungeonMonsterTargetScene(player, monsters);
            userInput = UtilManager.PlayerInput(0, monsters.Count);
            Monster userSelectedMonster = new Monster();                        //0을 입력시 몬스터 배열의 userInput조회시 -1을 조회시켜 에러발생하여 수정
            if (userInput == 0)
            {
                TargetMonster(player);
            }
            else
            {
                userSelectedMonster = monsters[userInput - 1];
                if (!userSelectedMonster.IsDie())
                {
                    int beforeMonsterHp = userSelectedMonster.hp;
                    var (playerDamage, hitType) = player.AttackCalculator(player.Attack()); //playerDamage의 최종 데미지 계산 
                    userSelectedMonster.hp = userSelectedMonster.hp - playerDamage < 0 ? 0 : userSelectedMonster.hp - playerDamage;
                    DisplayManager.DungeonPlayerAttackScene(player, userSelectedMonster, playerDamage, beforeMonsterHp,hitType);     //플레이어가 몬스터를 공격하는 장면
                    int next = UtilManager.PlayerInput(0, 0);
                    foreach (Monster monster in monsters)
                    {
                        if (monster.IsDie())
                        {
                            monsterCatches[Enum.GetName(typeof(MonsterType), monster.type)]++;
                        }
                        else
                        {
                            MonsterAttackPlayer(player, monster);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    PlayerAttackMonster(player);
                }
            }
        }

        public void MonsterAttackPlayer(Player player, Monster monster)
        {
            int beforePlayerHp = player.hp;
            player.hp -= monster.Attack();
            if (!player.Hpcheck())
            {
                DisplayManager.DungeonMonsterAttackScene(player, monster, beforePlayerHp);
                int userInput = UtilManager.PlayerInput(0, 0);
            }
        }

        public void Lose(Player player, int playerHpBeforeEnter)
        {
            DisplayManager.DungeonLoseResultScene(player, playerHpBeforeEnter);
        }

        public void Victory(Player player, int playerHpBeforeEnter)
        {
            int totalExp = monsters.Sum(monster => monster.level); // 몬스터 레벨 합산하여 경험치 추가

            // 경험치 추가 전 레벨 저장
            int beforeLevel = player.level;
            int expBeforeGain = player.exp;
            player.GainExp(totalExp);
            bool leveledUp = (beforeLevel < player.level);
            int expForNextLevel = player.GetExpForNextLevel();
            Reward reward = InventoryManager.instance.RewardInstnace;
            DisplayManager.DungeonWinResultScene(
        player, monsters.Count, playerHpBeforeEnter, reward.Gold(), reward.Potion(), reward.Item(),totalExp, expBeforeGain, expForNextLevel, leveledUp );
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

        public enum MonsterType
        {
            Minion = 0,
            Canon,
            VoidMonster
        }
    }
}
