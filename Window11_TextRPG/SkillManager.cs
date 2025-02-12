using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;
using static System.Net.Mime.MediaTypeNames;

namespace Window11_TextRPG
{
    public class SkillManager
    {
        public int AlphaStrike(Player player, Monster monster)
        {
            int damage = UtilManager.GetCeiling(player.atk) * 2;
            monster.hp = UtilManager.CalcDamage(monster.hp, damage);
            return damage;
        }

        public int DoubleStrike(Player player, Monster monster)
        {
            int damage = UtilManager.GetCeiling(player.atk * 1.5);
            monster.hp = UtilManager.CalcDamage(monster.hp, damage);
            return damage;
        }

        public int FireBall(Player player, Monster monster)
        {
            int damage = UtilManager.GetCeiling(player.atk) * 2;
            monster.hp = UtilManager.CalcDamage(monster.hp, damage);
            return damage;
        }

        public int Meteor(Player player, Monster monster)
        {
            int damage = UtilManager.GetCeiling(player.atk) * 3;
            monster.hp = UtilManager.CalcDamage(monster.hp, damage);
            return damage;
        }

        public int LifeSteal(Player player, Monster monster)
        {
            int damage = UtilManager.GetCeiling(player.atk);
            monster.hp = UtilManager.CalcDamage(monster.hp, damage);
            player.hp = player.hp + damage > player.maxhp ? player.maxhp : player.hp + damage
            return damage;
        }

        public int GoldAttack(Player player, Monster monster)
        {
            int damage = UtilManager.GetCeiling(player.atk) + (player.gold / 100);
            monster.hp = UtilManager.CalcDamage(monster.hp, damage);
            return damage;
        }

        public int PowerShot(Player player, Monster monster)
        {
            int damage = UtilManager.GetCeiling(player.atk) * 2;
            monster.hp = UtilManager.CalcDamage(monster.hp, damage);
            return damage;
        }

        public int MultiShot(Player player, Monster monster)
        {
            int damage = UtilManager.GetCeiling(player.atk * 1.5);
            monster.hp = UtilManager.CalcDamage(monster.hp, damage);
            return damage;
        }
    }
}
