using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    public class DungeonManager : IScene  
    {
        List<Monster> monsters = new List<Monster>();
        
        public void Enter()
        {
            //int input = UtilManager.PlayerInput(0,1);
        }

        public void SetMonsters(Player player)
        {
            int monsterCount = new Random().Next(1,5);  //1 ~ 4마리의 몬스터 생성을 위한 값
            for (int i = 0; i < monsterCount; i++)
            {
                int monsterType = new Random().Next(0,3);
                int minLevel = player.level - 2 <= 0 ? 1 : player.level - 2;
                int monsterLevel = new Random().Next(minLevel,player.level + 3);
                float monsterHp = new Random().Next(monsterLevel * (monsterType + 5),monsterLevel * (monsterType + 7));
                monsters.Add(new Monster(monsterLevel, monsterType, monsterHp));
            }
        }

        public void TargetMonster()
        {

        }

        public void PlayerAttackMonster(Player player, Monster monster)
        {
            //int minAttackDamage = Math.Ceiling(player.atk);
            //int attackDamage = new Random().Next( - 1,5);
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
