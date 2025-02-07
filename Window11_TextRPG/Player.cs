using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    public class Player : IMove
    {
        public string name { get; set; }
        public int level { get; set; }
        public string job { get; set; } 
        public float atk { get; set; } 
        public float def { get; set; } 

        public int maxhp { get; set; } 

        public int hp { get; set; } 
        public int gold { get; set; }

        public int exp { get; set; }

       
        public Player(string job, string name, int maxhp,float atk)
        {  //이름 ,직업, 체력, 최대 체력 , 공격력 ,방어력 ,돈, 경험치?
            this.name = name;
            this.job = job;
            this.maxhp = maxhp;
            this.atk = atk;

            this.level = 1;
            this.def = 5;
            this.hp = maxhp;
            this.gold = 1500;
            this.exp = 0;

        }
        public Player() { }


        public int Attack()
        {
           
            int playerAtk = UtilManager.GetCeiling((double) atk); ;
            int plusMinus = UtilManager.GetCeiling(playerAtk * 0.1);
            int attackDamage = UtilManager.getRandomInt(playerAtk - plusMinus, playerAtk + plusMinus);

            return attackDamage;
        }

        // public Item weapon { get; set; } = null;
        // public Item armor { get; set; } = null;

        public bool Hpcheck()
        {
            if (hp <= 0)
            {
                return true;
            }

            return false;
        }

    }
}


