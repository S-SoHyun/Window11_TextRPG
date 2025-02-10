using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window11_TextRPG;

namespace Window_11_TEXTRPG
{
    public class InventoryManager : IScene
    {
        // Singleton
        private InventoryManager() { }
        public static InventoryManager? instance;

        public static InventoryManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InventoryManager();
                return instance;
            }
        }


        // 장착 가능 아이템 생성
        public List<MountableItem> mountableItems = new List<MountableItem>()
        {
             new MountableItem()
            {
                Name = "수련자의 갑옷",
                Description = "수련에 도움을 주는 갑옷입니다. ",
                Price = 1000,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 4,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "무쇠갑옷",
                Description = "무쇠로 만들어져 튼튼한 갑옷입니다.",
                Price = 2000,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 9,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "스파르타의 갑옷",
                Description = "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",
                Price = 3500,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 15,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "낡은 검",
                Description = "쉽게 볼 수 있는 낡은 검 입니다. ",
                Price = 600,
                Type = ITEMTYPE.WEAPON,

                Attack = 5,
                Defense = 0,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "청동 도끼",
                Description = "어디선가 사용됐던거 같은 도끼입니다. ",
                Price = 1500,
                Type = ITEMTYPE.WEAPON,

                Attack = 10,
                Defense = 0,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "스파르타의 창",
                Description = "스파르타의 전사들이 사용했다는 전설의 창입니다. ",
                Price = 2500,
                Type = ITEMTYPE.WEAPON,

                Attack = 20,
                Defense = 0,
                Own = false,
                Equip = false
            },
        };


        // 포션 생성
        public PotionItem potion = new PotionItem()
        {
            Name = "HP 포션",
            Description = "HP를 30 회복할 수 있는 포션입니다.",
            Price = 500,
            Type = ITEMTYPE.POTION,

            Count = 3,
            Heel = 30
        };


        // 1. 상점에서 산 아이템 리스트, 
        // 2. 상점에서 샀고 타입이 웨폰인 리스트,
        // 3. 상점에서 샀고 타입이 아머인 리스트 세분화해서 생성
        List<MountableItem> ownItems = new List<MountableItem>();
        List<MountableItem> ownWeaponItems = new List<MountableItem>();
        List<MountableItem> ownArmorItems = new List<MountableItem>();


        public void Enter()
        {
            while (true)
            {
                int input = UtilManager.PlayerInput(0, 1);

                switch (input)
                {
                    case 0:
                        DisplayManager.MainScene();
                        break;
                    case 1:
                        //DisplayManager.EquipmentScene();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

            }
        }


        // 상점에서 산 아이템들 따로 리스트에 넣기
        public void DivisionOwnItem()
        {
            for (int i = 0; i < mountableItems.Count; i++)
            {
                if (mountableItems[i].Own)
                {
                    ownItems.Add(mountableItems[i]);
                }
            }
            
        }


        // 상점에서 산 아이템들 중에서 타입에 따라 또 나누기 (웨폰 / 아머)
        public void DivisionType()
        {
            for (int i = 0; i < ownItems.Count; i++)
            {
                if (ownItems[i].Type == ITEMTYPE.WEAPON)
                {
                    ownWeaponItems.Add(ownItems[i]);
                }
                else if (ownItems[i].Type == ITEMTYPE.ARMOR)
                {
                    ownArmorItems.Add(ownItems[i]);
                }
            }
        }


        public void Equip(int input)    // 장착 (타입에 따라 중복 장착이 안 되도록)
        {
            MountableItem select = ownItems[input - 1];
            MountableItem equipped = null;
            DivisionType();


            // 1. 고른 게 웨폰이라면 weaponItems에서 장착된 게 있는지 확인 후 장착
            if (select.Type == ITEMTYPE.WEAPON)
            {
                // ownWeaponItems에서 equip이 true인 것 찾기
                for (int i = 0; i < ownWeaponItems.Count; i++)
                {
                    if (ownWeaponItems[i].Equip)
                    {
                        equipped = ownWeaponItems[i];
                        break;
                    }
                }

                if (equipped != null)
                {
                    Unequip(equipped);
                    select.Equip = true;
                }
                else
                    select.Equip = true;

            }
            // 2. 고른 게 아머라면 armorItems에서 장착된 게 있는지 확인 후 장착
            else if (select.Type == ITEMTYPE.ARMOR)
            {
                // ownArmorItems에서 equip이 true인 것 찾기
                for (int i = 0; i < ownArmorItems.Count; i++)
                {
                    if (ownArmorItems[i].Equip)
                    {
                        equipped = ownArmorItems[i];
                        break;
                    }
                }

                if (equipped != null)
                {
                    Unequip(equipped);
                    select.Equip = true;
                }
                else
                    select.Equip = true;

            }
        }


        public void Unequip(MountableItem equipped)
        {
            equipped.Equip = false;
        }



        public void UsePotion(PotionItem potion)    // 물약 사용
        {
            if (potion.Count > 0)      // 만약 물약이 한 개 이상 있다면
            {
                potion.Count--;
                //Player.hp += potion.Heel;                                    // !player에서 오류. 확인!
                Console.WriteLine("회복을 완료했습니다.");
            }
            else
                Console.WriteLine("포션이 부족합니다.");
        }


        // ↓ DisplayManager에 넣을 함수 ↓ //

        public void InventoryScene()
        {
            Console.Write("[인벤토리]");

            Console.WriteLine();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            Console.WriteLine();
            //ViewInventoryItem();    // DisplayManager?


            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");

            Console.WriteLine();
        }

        public void EquipmentScene()
        {
            Console.Write("[인벤토리- 장착 관리]");

            Console.WriteLine();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            Console.WriteLine();
            //ViewEquipItem();    // DisplayManager?

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("0. 나가기");


        }


        public void ViewInventoryItem()     // 인벤토리 창에서 보일 아이템 목록
        {
            for (int i = 0; i < ownItems.Count; i++)
            {
                string typeName = (ownItems[i].Attack > 0) ? "공격력" : "방어력";
                int value = (ownItems[i].Attack > 0) ? ownItems[i].Attack : ownItems[i].Defense;

                if (ownItems[i].Equip)
                {
                    Console.Write("- [E]");
                }
                else
                {
                    Console.Write("- ");
                }
                Console.WriteLine($"{ownItems[i].Name}  |  {typeName} + {value}  |  {ownItems[i].Description}");
            }
        }


        public void ViewEquipItem()    // 장착관리 창에서 보일 아이템 목록
        {
            for (int i = 0; i < ownItems.Count; i++)
            {
                string typeName = (ownItems[i].Attack > 0) ? "공격력" : "방어력";
                int value = (ownItems[i].Attack > 0) ? ownItems[i].Attack : ownItems[i].Defense;

                if (ownItems[i].Equip)
                {
                    Console.Write($"- {i + 1} [E]");
                }
                else
                {
                    Console.Write($"- {i + 1} ");
                }
                Console.WriteLine($"{ownItems[i].Name}  |  {typeName} + {value}  |  {ownItems[i].Description}");
            }
        }





    }
}


