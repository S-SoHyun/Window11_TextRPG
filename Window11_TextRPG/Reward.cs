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
        public Reward()
        {
            // 보상 아이템 초기화
            init_rewardWeapon();
            init_rewardArmor();
            init_rewardPotion();
            init_rewardGold();
        }

        Func<int, int, int> ranFunc = UtilManager.getRandomInt;
        // 보상 아이템 설정
        List<MountableItem> rewardWeapon = new List<MountableItem>();
        List<MountableItem> rewardArmor = new List<MountableItem>();
        List<PotionItem> rewardPotion = new List<PotionItem>();
        List<int> rewardGold = new List<int>();

        // 참조
        List<MountableItem> mountableItems = InventoryManager.Instance.mountableItems;
        List<PotionItem> potionItems = InventoryManager.Instance.potions;

        // 50%로 아이템 반환 및 적용
        MountableItem? Item(Player player)
        {
            MountableItem item;
            // 장비 아이템 얻을 확률 
            int odds = 50;
            int random = ranFunc(0, 100);
            if (odds > random)
            {
                // 무기, 방어구 반반
                if (5 < ranFunc(0, 10))
                {
                    item = rewardItem(rewardWeapon, mountableItems);
                }
                else
                {
                    item = rewardItem(rewardArmor, mountableItems);
                }
                return item;
            }
            return null;
        }

        // Gold 랜덤 반환 및 적용
        int Gold(Player player)
        {
            Random random = new Random();
            int gold = UtilManager.getRandomInt(0, rewardGold.Count);

            player.gold += gold;

            return gold; // 화면 출력용 반환
        }

        // 포션 갯수 반환 및 적용
        int Potion(Player player)
        {
            int count = ranFunc(1, 5);
            PotionItem potion = rewardPotion[0];

            // 포션을 가지고 있는지
            if (!potionItems.Contains(potion))
            {
                potionItems.Add(potion);
            }

            potion.Count += count;

            return count; // 포션 갯수 반환
        }


        T rewardItem<T>(List<T> reward, List<T> result) where T : MountableItem
        {
            T item;
            do
            {
                item = reward[ranFunc(0, reward.Count)];
                item.Own = true;

                // 무한 반복 방지
                int count = 0;
                foreach (T item2 in reward)
                {
                    if (item2.Own)
                    {
                        count++;
                    }
                }
                if (count == reward.Count)
                    break;
            }
            // 아이템 중복 검사. 중복이면 한번 더 
            while (result.Contains(item));

            return item;
        }


        void init_rewardGold()
        {
            rewardGold.Add(100);
            rewardGold.Add(200);
            rewardGold.Add(300);
            rewardGold.Add(400);
            rewardGold.Add(500);
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
        void init_rewardArmor()
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
        void init_rewardPotion()
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
