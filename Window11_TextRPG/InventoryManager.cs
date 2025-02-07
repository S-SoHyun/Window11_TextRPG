using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window_11_TEXTRPG
{
    public class InventoryManager : IScene
    {

        // 장착 가능 아이템, 물약 아이템 리스트 생성
        List<MountableItem> mountableItems = new List<MountableItem>();
        List<PotionItem> potions = new List<PotionItem>();

      
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



        public void Equip()   // 장착
        {

        }


        public void UnEquip()    // 장착 해제
        {

        }


        public void UsePotion()    // 물약 사용
        {
            // 만약 물약을 사용하겠다하면
                // 플레이어가 갖고 있는 물약 카운트가 줄고
                // hp가 heal 됨

            //if (potionItem[0] > 0)
            //{
            //    PotionItem.count--;
            //    Player.hp += potionItem.heal;
            //}
        }

        public void ViewEquipItem()    // 장착 아이템 보기
        {

        }

        public void Enter()
        {
            
        }




        // 무기 종류에 따라도 다르게 동작?
        //만약 고른 게 종류가 무기라면
        //    근데 그 고른 종류를 이미 장착했다면
        //    이미 장착돼있는 건 빼기 
        //    고른 걸 장착하기
        //    장착이 안 돼있다면
        //    고른 걸 장착하기
        //만약 고른 게 종류가 아머라면
        //    근데 그 고른 종류를 이미 장착했다면
        //    이미 장착돼있는 건 빼기
        //    고른 걸 장착하기
        //    장착이 안 돼있다면
        //    고른 걸 장착하기
        // → 구현할 때 하나로 묶어서 정리

    }
}

