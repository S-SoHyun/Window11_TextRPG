using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window11_TextRPG
{
    public class Monster
    {
        string name;
        int level;
        string type;
        int hp;

        public Monster(string name, int level, string type, int hp) 
        {
            this.name = name;
            this.level = level;
            this.type = type;
            this.hp = hp;
        }

        //hp 0 이하인지 체크
        public bool IsDie()
        {
            if (hp <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
