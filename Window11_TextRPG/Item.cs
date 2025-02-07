using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window_11_TEXTRPG
{
    public enum ITEMTYPE
    {
        WEAPON,
        ARMOR,
        POTION,
    }
    public abstract class Item
    {
        protected string? name;
        protected string? description;
        protected int price;
        protected ITEMTYPE type;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        public ITEMTYPE Type
        {
            get { return type; }
            set { type = value; }
        }
    }

    // 장착 가능한 아이템
    public class MountableItem : Item
    {
        private int attack;
        private int defense;
        private bool own;
        private bool equip;

        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }
        public bool Own
        {
            get { return own; }
            set { own = value; }
        }
        public bool Equip
        {
            get { return equip; }
            set { equip = value; }
        }

    }

    // 포션 아이템
    public class PotionItem : Item
    {
        private int count;
        private int heel;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        public int Heel
        {
            get { return heel; }
            set { heel = value; }
        }
    }




    
}
