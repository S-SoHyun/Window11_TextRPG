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


        // 장착 가능 아이템, 물약 아이템 리스트 생성
        public List<MountableItem> mountableItems = new List<MountableItem>();
        public List<PotionItem> potions = new List<PotionItem>();


        // 장착 가능 아이템 - 웨폰, 아머를 나누기 위한 리스트 생성
        List<MountableItem> weaponItems = new List<MountableItem>();
        List<MountableItem> armorItems = new List<MountableItem>();



        public void AddItem()
        {
            // 1. 장착할 수 있는 아이템 추가
            mountableItems.Add(new MountableItem());
            mountableItems.Add(new MountableItem());
            mountableItems.Add(new MountableItem());

            //2. 포션 추가
            potions.Add(new PotionItem());
            potions.Add(new PotionItem());
            potions.Add(new PotionItem());
        }


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


        // mountableItems 리스트에서 weapon이랑 armor 타입에 따라 나누기
        public void DivisionType()
        {
            for (int i = 0; i < mountableItems.Count; i++)
            {
                if (mountableItems[i].Type == ITEMTYPE.WEAPON)
                {
                    weaponItems.Add(mountableItems[i]);
                }
                else if (mountableItems[i].Type == ITEMTYPE.ARMOR)
                {
                    armorItems.Add(mountableItems[i]);
                }
            }
        }


        // 코딩하다보니 종류별로 이큅되게 했는데 이렇게 하면 불편할 것 같음. equip, unequip으로 해야 되지 않을까?

        public void WeaponEquip(int input)    // 웨폰아이템 장착
        {
            DivisionType();
            MountableItem select = mountableItems[input - 1];
            MountableItem equipped = null;

            for (int i = 0; i < weaponItems.Count; i++)
            {
                if (weaponItems[i].Equip)
                {
                    equipped = weaponItems[i];
                    break;
                }
            }

            if (equipped == null)     // equipped가 여전히 null이라면 장착하고 아니면 그거 해제하고 고른거 장착하도록.
            {
                select.Equip = true;
                //스탯창에서 atk, def 추가되는 코드
            }
            else
            {
                equipped.Equip = false;
                select.Equip = true;
                //스탯창에서 atk, def 추가되는 코드
            }
        }


        public void ArmorEquip(int input)    // 아이템 장착
        {
            DivisionType();
            MountableItem select = mountableItems[input - 1];
            MountableItem equipped = null;

            for (int i = 0; i < armorItems.Count; i++)
            {
                if (armorItems[i].Equip)
                {
                    equipped = armorItems[i];
                    break;
                }
            }

            if (equipped == null)     // equipped가 여전히 null이라면 장착하고 아니면 그거 해제하고 고른거 장착하도록.            
                select.Equip = true;
            else
            {
                equipped.Equip = false;
                select.Equip = true;
            }
        }


        public void UsePotion(PotionItem potion)    // 물약 사용
        {
            if (potion.Count > 0)      // 만약 물약이 한 개 이상 있다면
            {
                potions.RemoveAt(0);             // 물약 리스트에서 0번 항목 제거
                //Player.hp += potion.heal;
                Console.WriteLine("회복을 완료했습니다.");
            }
            else
            {
                Console.WriteLine("포션이 부족합니다.");
            }
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
            for (int i = 0; i < mountableItems.Count; i++)
            {
                string typeName = (mountableItems[i].Attack > 0) ? "공격력" : "방어력";
                int value = (mountableItems[i].Attack > 0) ? mountableItems[i].Attack : mountableItems[i].Defense;

                if (mountableItems[i].Equip)
                {
                    Console.Write("- [E]");
                }
                else
                {
                    Console.Write("- ");
                }
                Console.WriteLine($"{mountableItems[i].Name}  |  {typeName} + {value}  |  {mountableItems[i].Description}");
            }
        }


        public void ViewEquipItem()    // 장착관리 창에서 보일 아이템 목록
        {
            for (int i = 0; i < mountableItems.Count; i++)
            {
                string typeName = (mountableItems[i].Attack > 0) ? "공격력" : "방어력";
                int value = (mountableItems[i].Attack > 0) ? mountableItems[i].Attack : mountableItems[i].Defense;

                if (mountableItems[i].Equip)
                {
                    Console.Write($"- {i + 1} [E]");
                }
                else
                {
                    Console.Write($"- {i + 1} ");
                }
                Console.WriteLine($"{mountableItems[i].Name}  |  {typeName} + {value}  |  {mountableItems[i].Description}");
            }
        }





    }
}


