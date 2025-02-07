using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window11_TextRPG
{
    public class Monster
    {
        public string name { get; set; }
        public int level { get; set; }
        public int type { get; set; }
        public float hp { get; set; }

        public Monster(int level, int type, float hp)
        {
            string name = "";
            switch (type)
            {
                case 0:
                    name = "미니언";
                    break;
                case 1:
                    name = "대포미니언";
                    break;
                case 2:
                    name = "공허충";
                    break;
            }
            this.name = name;
            this.level = level;
            this.type = type;
            this.hp = hp;
        }

        public Monster(string name, int level, int type, float hp) 
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
