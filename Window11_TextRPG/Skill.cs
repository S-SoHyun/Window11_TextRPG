using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window11_TextRPG
{
    public class Skill
    {
        public string name { get; set; }
        public string description { get; set; }
        public int mp { get; set; }
        public int type { get; set; }
        Func<Player, Monster, int> action { get; set; }

        // action 포함 생성자
        public Skill(string name, string description, int mp, int type, Func<Player, Monster, int> action)
        {
            this.name = name;
            this.description = description;
            this.mp = mp;
            this.type = type;
            this.action = action;
        }
        // action 미포함 생성자
        public Skill(string name, string description, int mp, int type)
        {
            this.name = name;
            this.description = description;
            this.mp = mp;
            this.type = type;
        }

        // action을 나중에 설정 가능
        public void SetAction(Func<Player, Monster, int> action)
        {
            this.action = action;
        }


        public int Invoke(Player player, Monster monster)
        {
            int result = 0;

            if (action != null)
            {
                result = action(player, monster);
            }
            else
            {
                Console.WriteLine("Action 값이 없다.");  // Debug용
            }
            return result;
        }
    }


}
