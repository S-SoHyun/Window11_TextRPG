using System;
using System.Collections;
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
        public Reward(List<MountableItem> _mountableItems, PotionItem _potion)
        {
            // 보상 아이템 초기화
            init_rewardWeapon();
            init_rewardArmor();
            init_rewardGold();

            this.mountableItems = _mountableItems;
            this.potion = _potion;
        }

        Func<int, int, int> ranFunc = UtilManager.getRandomInt;

        // 보상 아이템 설정
        List<MountableItem> rewardWeapon = new List<MountableItem>();
        List<MountableItem> rewardArmor = new List<MountableItem>();
        List<int> rewardGold = new List<int>();

        // 참조
        List<MountableItem> mountableItems;
        PotionItem potion;

        // 50%로 아이템 반환 및 적용
        public MountableItem? Item()
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

        // 지정 아이템 반환
        public MountableItem? GetItem(string name)
        {
            // 보상 장비(무기, 방어구) 리스트
            List<MountableItem> rewards = rewardArmor.Concat(rewardWeapon).ToList();

            for (int i = 0; i < rewards.Count; i++)
            {
                if (name == rewards[i].Name)
                    return rewards[i];
            }

            // 없으면 null 반환
            return null;
        }

        // 지정 아이템 획득
        public void SetItem(MountableItem item)
        {
            mountableItems.Add(item);
            item.Own = true;
        }

        // Gold 랜덤 반환 및 적용
        public int Gold()
        {
            Player player = PlayerManager.Instance._Player;
            int gold = rewardGold[UtilManager.getRandomInt(0, rewardGold.Count)];

            player.gold += gold;

            return gold; // 화면 출력용 반환
        }

        // 포션 갯수 반환 및 적용
        public int Potion()
        {
            int count = ranFunc(1, 6);
 
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

            // 보상아이템 인벤토리로 추가
            result.Add(item);

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
                Name = "흙 검",
                Description = "흙으로 만든 검",
                Price = 100,
                Type = ITEMTYPE.WEAPON,

                Attack = 3,
                Defense = 0,
                Own = false,
                Equip = false
            });
            rewardWeapon.Add(new MountableItem()
            {
                Name = "구리 검",
                Description = "낡은 검보다 강하며 철 검보다 약하다.",
                Price = 150,
                Type = ITEMTYPE.WEAPON,

                Attack = 5,
                Defense = 0,
                Own = false,
                Equip = false
            });
            rewardWeapon.Add(new MountableItem()
            {
                Name = "은 검",
                Description = "현존하는 가장 강한 검",
                Price = 200,
                Type = ITEMTYPE.WEAPON,

                Attack = 10,
                Defense = 0,
                Own = false,
                Equip = false
            });
            rewardWeapon.Add(new MountableItem()
            {
                Name = "나무 활",
                Description = "가장 약한 활",
                Price = 200,
                Type = ITEMTYPE.WEAPON,

                Attack = 10,
                Defense = 0,
                Own = false,
                Equip = false
            });
            rewardWeapon.Add(new MountableItem()
            {
                Name = "철 활",
                Description = "나무 활 보다 강력하다",
                Price = 200,
                Type = ITEMTYPE.WEAPON,

                Attack = 10,
                Defense = 0,
                Own = false,
                Equip = false
            });
            rewardWeapon.Add(new MountableItem()
            {
                Name = "칠흑나무 활",
                Description = "칠흑나무로 만든 활",
                Price = 200,
                Type = ITEMTYPE.WEAPON,

                Attack = 10,
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
                Price = 50,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 20,
                Own = false,
                Equip = false
            });
            rewardArmor.Add(new MountableItem()
            {
                Name = "워셔블밀라노 스웨터",
                Description = "깔끔한 짜임과 신축성이 있는 원단",
                Price = 100,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 25,
                Own = false,
                Equip = false
            });
            rewardArmor.Add(new MountableItem()
            {
                Name = "코튼엑스트라윔",
                Description = "코튼 100%",
                Price = 150,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 30,
                Own = false,
                Equip = false
            });
            rewardArmor.Add(new MountableItem()
            {
                Name = "플러피얀후리스풀집 재킷",
                Description = "이불을 덮은 듯한 따뜻함",
                Price = 200,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 35,
                Own = false,
                Equip = false
            });
        }

    }
}
