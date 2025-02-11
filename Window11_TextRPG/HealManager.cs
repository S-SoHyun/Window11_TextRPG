using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    public class HealManager: IScene
    {
        // Singleton
        private HealManager()
        {
        //    reward = new Reward(mountableItems, potion);
        }
        public static HealManager? instance;

        public static HealManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new HealManager();
                return instance;
            }
        }


        public void Enter()
        {
            
        }





    }
}
