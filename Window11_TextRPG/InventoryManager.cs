using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Window11_TextRPG;

namespace Window_11_TEXTRPG
{
    public class InventoryManager : IScene
    {
        // Singleton
        private InventoryManager() { }
        private static InventoryManager? instance;

        InventoryManager Instance
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


        // ↓ 미구현. 로직짜면서 좀 더 깔끔하게 쓸 방법 고민중

        public void Enter()
        {
            //while (true)
            //{

            //    int result = UtilManager.PlayerInput(0, 1);
            //    switch (result)
            //    {
            //        case 0:    // 로비로 나가기

            //            break;
            //        case 1:    // 장착 관리 창으로 가기
            //            DisplayManager.EquipmentScene(player, ?);
            //            break;
            //        default:
            //            Console.WriteLine("잘못된 입력입니다.");
            //            break;
            //    }
            //    }
        }



        //public void Equip(MountableItem select)
        //{
        //    if (select.Type = ITEMTYPE.WEAPON && select.Equip = true)
        //    {
        //        UnEquip(select);
        //        select.Equip = true;
        //    }
        //    else
        //    {
        //        select.Equip = true;
        //    }

        //    if (select.Type = ITEMTYPE.ARMOR && select.Equip = true)
        //    {
        //        UnEquip(select);
        //        select.Equip = true;
        //    }
        //    else
        //    {
        //        select.Equip = true;
        //    }
        //}


        //public void UnEquip(Item select)    // 장착 해제
        //{
        //    select.Equip = false;
        //}


        //public void UsePotion(PotionItem potion)    // 물약 사용
        //{
        //    if (potion.Count > 0)      // 만약 물약이 한 개 이상 있다면
        //    {
        //        potion[0].Count--;     // 플레이어가 갖고 있는 물약 카운트가 줄고
        //        Player.hp += potion.heel;      // hp가 potion의 heal만큼 됨
        //    }
        //    else
        //    {
        //        Console.WriteLine("보유한 포션이 부족합니다.");
        //    }
        //}


        //public void ViewinventoryItem()    // 인벤토리 창에서 아이템 보기
        //{
        //    for (int i = 0; i < mountableItems.Count; i++)
        //    {
        //        Item select = mountableItems[i+1];
        //        if (select.Equip)
        //        {
        //            Console.Write("[E] ");
        //        }
        //        Console.WriteLine($"{select.name} | {select.Type} +{select.?} | {select.Description}");
        //    }
        //}

        //public void ViewEquipItem()    // 장착관리 창에서 아이템 보기
        //{
        //    for (int i = 0; i < mountableItems.Count; i++)
        //    {
        //        if (mountableItems[i].Equip)
        //        {
        //            Console.Write("[E] ");
        //        }
        //        Console.WriteLine($"{i + 1}. {mountableItems[i]}"); // 양식 다듬어서 (미완)
        //    }
        //}



        // DisplayManager에 넣을 함수 //

        //public void equipdisplay()
        //{
        //    Console.Write("[인벤토리]");

        //    AddBlankLine();
        //    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

        //    AddBlankLine();
        //    Console.WriteLine("[아이템 목록]");

        //    AddBlankLine();
        //    ViewEquipItem(items, true);

        //    AddBlankLine(2);
        //    Console.WriteLine("1. 아이템 구매");
        //    Console.WriteLine("0. 나가기");

        //    InputInduction();
        //}



        //public void equipmentdisplay()
        //{
        //    Console.Write("[인벤토리- 장착 관리]");

        //    AddBlankLine();
        //    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

        //    AddBlankLine();
        //    Console.WriteLine("[아이템 목록]");

        //    AddBlankLine();
        //    ViewEquipItem(items, true);    // 여기에서 앞에 번호 붙인 거 메서드 하나 따로 만들어야 될 듯? 아니면 좋은 방법 강구

        //    AddBlankLine(2);
        //    Console.WriteLine("0. 나가기");

        //    InputInduction();
        //}
    }
}


