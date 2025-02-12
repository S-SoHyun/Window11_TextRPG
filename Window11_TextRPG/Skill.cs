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
        Func<int> action { get; set; }

        //action 함수 추가 필요
        public Skill(string name, string description, int mp, int type)
        {
            this.name = name;
            this.description = description;
            this.mp = mp;
            this.type = type;
        }

        public void invoke()
        {
            if (action != null)
            {
                action();
            }
        }
    }
}
