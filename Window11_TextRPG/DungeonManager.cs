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
            Player player = new Player("전사", "test", 130, 10);      //테스트용 임시코드 player 부분은 
            SetMonsters(player);
            DisplayManager.DungeonScene(player,monsters);
            PlayerAttackMonster(player);
            for (int i = 0; i < monsters.Count; i++)
            {
                Console.WriteLine($"Lv.{monsters[i].level} {monsters[i].name} HP {monsters[i].hp}");
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

        public void PlayerAttackMonster(Player player)
        {
            int userInput = UtilManager.PlayerInput(0, monsters.Count);
            Monster userSelectedMonster = monsters[userInput - 1];
            if (!userSelectedMonster.IsDie())
            {
                int beforeMonsterHp = userSelectedMonster.hp;
                int playerDamage = player.Attack();
                userSelectedMonster.hp = userSelectedMonster.hp - playerDamage < 0 ? 0 : userSelectedMonster.hp - playerDamage;
            }
            else
            {
                PlayerAttackMonster(player);
            }
        }

        public void MonsterAttackPlayer()
        {

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
