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
        public Item weapon { get; set; } = null;
        public Item armor { get; set; } = null;


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

        public (int finalDamage, int hit) AttackCalculator(int attackDamage)
        {
            int hit = 0;
            int finalDamage = 0;
            int atkhit = UtilManager.getRandomInt(0, 101);

            if (atkhit < 10)
            {
                hit = 1;  // 빗나감
                finalDamage = 0;
            }
            else if (atkhit < 25)
            {
                hit = 2;  // 치명타
                finalDamage = UtilManager.GetCeiling(attackDamage * 1.6);
            }
            else
            {
                hit = 3;  // 기본 적중
                finalDamage = attackDamage;
            }

            return (finalDamage, hit); // 두 개의 값을 튜플로 반환
        }



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


