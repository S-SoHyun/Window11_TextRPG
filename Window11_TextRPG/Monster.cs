using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    public class Monster :IMove 
    {
        public string name { get; set; }
        public int level { get; set; }
        public int type { get; set; }
        public int hp { get; set; }

        public Monster(int level, int type, int hp)
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

        public Monster(string name, int level, int type, int hp) 
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

        public int Attack()
        {

           int attackDamage = 0;

            switch (type)
            {
                case 0: // 미니언
                    attackDamage = 5 + (level * 2);
                    break;
                case 1: // 대포미니언
                    attackDamage = 10 + (level * 3);
                    break;
                case 2: // 공허충
                    attackDamage = 15 + (level * 4);
                    break;
                default:
                    attackDamage = 3 + (level * 1);
                    break;
            }
            return attackDamage;
        }
    }
}
