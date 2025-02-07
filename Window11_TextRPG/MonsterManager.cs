using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    internal class DungeonManager : IScene  
    {
        List<Monster> monsters = new List<Monster>();
        
        public void Enter()
        {
            //int input = UtilManager.PlayerInput(0,1);
        }

        public void SetMonsters()
        {

        }

        public void TargetMonster()
        {

        }

        public void AttackMonster()
        {

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
