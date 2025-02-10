using System;
using System.Collections.Generic;
using System.Linq;
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
        private DungeonManager() { }

        List<Monster> monsters = new List<Monster>();

        
        
        public void Enter()
        {
            Player player = new Player("전사", "test", 130, 10);      //테스트용 임시코드 player
            int monsterDieCnt = 0;
            int playerHpBeforeEnter = player.hp;
            SetMonsters(player);
            while (!player.Hpcheck() && monsterDieCnt != monsters.Count)
            {
                DisplayManager.DungeonScene(player, monsters);
                PlayerAttackMonster(player,ref monsterDieCnt);
            }
            if (player.Hpcheck())
            {
                DisplayManager.DungeonLoseResultScene(player, playerHpBeforeEnter);
            }
            else if (monsterDieCnt == monsters.Count) 
            {
                DisplayManager.DungeonWinResultScene(player, monsters.Count, playerHpBeforeEnter);
            }
            switch (UtilManager.PlayerInput(0,0))
            {
                case 0:
                    //상위 메뉴로 이동
                    break;
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

        public void PlayerAttackMonster(Player player, ref int monsterDieCnt)
        {
            int userInput = UtilManager.PlayerInput(0, monsters.Count);
            Monster userSelectedMonster = new Monster();                        //0을 입력시 몬스터 배열의 userInput조회시 -1을 조회시켜 에러발생하여 수정
            if (userInput == 0)
            {
                //상위 메뉴로
            }
            else
            {
                userSelectedMonster = monsters[userInput - 1];
                if (!userSelectedMonster.IsDie())
                {
                    int beforeMonsterHp = userSelectedMonster.hp;
                    var (playerDamage, hitType) = player.AttackCalculator(player.Attack()); //playerDamage의 최종 데미지 계산 
                    userSelectedMonster.hp = userSelectedMonster.hp - playerDamage < 0 ? 0 : userSelectedMonster.hp - playerDamage;
                    DisplayManager.DungeonPlayerAttackScene(player, userSelectedMonster, playerDamage, beforeMonsterHp);     //플레이어가 몬스터를 공격하는 장면
                    int next = UtilManager.PlayerInput(0, 0);
                    foreach (Monster monster in monsters)
                    {
                        if (monster.IsDie())
                        {
                            monsterDieCnt++;
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
                    PlayerAttackMonster(player, ref monsterDieCnt);
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

        public void Lose()
        {

        }

        public void Victory()
        {

        }

        enum MonsterType
        {
            Minion = 0,
            Canon,
            VoidMonster
        }
    }
}
