using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    public class Reward
    {
        // 보상 아이템 설정
        List<MountableItem> rewardWeapon = new List<MountableItem>();
        List<MountableItem> rewardArmor = new List<MountableItem>();
        List<PotionItem> rewardPotion = new List<PotionItem>();

        // 보상 아이템 초기화
        public void init()
        {
            init_rewardWeapon();
            init_rewardArmor();
            init_rewardPotion();
        }

        void Item(Player player)
        {

        }

        void Gold(Player player)
        {

        }

        void Level()
        {

        }

        void init_rewardWeapon()
        {
            rewardWeapon.Add(new MountableItem()
            {
                Name = "짱돌",
                Description = "흔한 짱돌이다.",
                Price = 10,
                Type = ITEMTYPE.WEAPON,

                Attack = 5,
                Defense = 0,
                Own = false,
                Equip = false
            });
            rewardWeapon.Add(new MountableItem()
            {
                Name = "나뭇가지",
                Description = "흔한 나뭇가지다.",
                Price = 20,
                Type = ITEMTYPE.WEAPON,

                Attack = 3,
                Defense = 0,
                Own = false,
                Equip = false
            });
            rewardWeapon.Add(new MountableItem()
            {
                Name = "낡은 검",
                Description = "낡은 검이다.",
                Price = 100,
                Type = ITEMTYPE.WEAPON,

                Attack = 3,
                Defense = 0,
                Own = false,
                Equip = false
            });
        }
        public void init_rewardArmor()
        {
            rewardArmor.Add(new MountableItem()
            {
                Name = "풀옷",
                Description = "대충 잡초로 엮은 옷이다.",
                Price = 15,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 5,
                Own = false,
                Equip = false
            });
            rewardArmor.Add(new MountableItem()
            {
                Name = "유니클로 셔츠",
                Description = "유니클로 셔츠다.",
                Price = 100,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 20,
                Own = false,
                Equip = false
            });
        }
        public void init_rewardPotion()
        {
            rewardPotion.Add(new PotionItem()
            {
                Name = "체력 포션",
                Description = "(비매품)체력을 채워준다.",
                Price = 0,
                Type = ITEMTYPE.POTION,

                Count = 1,
                Heel = 30,

            });
        }

    }
}
