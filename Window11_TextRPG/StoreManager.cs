using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    internal class StoreManager : IScene
    {
        // Singleton
        private StoreManager() { }
        private static StoreManager? instance;
        public static StoreManager GetInst()
        {
            if (instance == null)
                instance = new StoreManager();
            return instance;
        }

        // 임시 변수들
        Player player = new Player();    
        List<MountableItem> items;
        //InventoryManager

        // GameManager에서 접근하는 곳
        public void Enter()
        {
            // StoreScene 진입
            StoreScene();
        }


        private void StoreScene()
        {

            while (true)
            {
                // StoreScene 출력
                DisplayManager.StoreScene(player, items);
                int result = UtilManager.PlayerInput(1, 3);
                switch (result)
                {
                    case 1: // 구매 페이지 진입
                        DisplayManager.StoreBuyScene(player, items);
                        BuyItemScene();
                        break;

                    case 2: // 판매 페이지 진입
                        DisplayManager.StoreBuyScene(player, items);
                        SellItemScene();
                        break;

                    case 3: // 로비 페이지 진입

                        break;
                }
            }
        }


        private void BuyItemScene()
        {
            while (true)
            {   
                // DiplayManager 접근
                int result = UtilManager.PlayerInput(1, items.Count());
                switch (result)
                {
                    case 1: // 구매 페이지 진입  

                        break;

                    case 2: // 판매 페이지 진입

                        break;

                    case 3: // 로비 페이지 진입

                        break;
                }
            }
        }

        private void SellItemScene()
        {
            // DiplayManager 접근
        }

        private void BuyItem(int idx)
        {
            // 구매 불가
            if (items[idx - 1].Own)
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                Thread.Sleep(1000);
            }
            // 구매 가능
            else
            {
                // Gold 충분
                if (items[idx - 1].Price <= player.gold)
                {
                    items[idx - 1].Own = true;
                    player.gold -= items[idx - 1].Price;
                    Console.WriteLine("구매를 완료했습니다.");
                    Thread.Sleep(1000);
                }
                // Gold 부족
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                    Thread.Sleep(1000);
                }
            }
        }

        private void SellItem(int idx)
        {
            //EquipmentScene equipmentScene = EquipmentScene.GetInst();
            MountableItem item = items[idx - 1];
            // 판매 불가
            if (!item.Own)
            {
                Console.WriteLine("소유하지 않은 아이템입니다.");
                Thread.Sleep(1000);
            }
            // 판매 가능
            else
            {
                // 장착 중이라면 장착 해제
                if (item.Equip)
                {
                    // 아이템 성능 만큼 캐릭터 성능 하향
                    //equipmentScene.MountItem(item, false);
                }
                item.Own = false;
                player.gold += (int)((float)item.Price * 0.85f);
                Console.WriteLine("판매를 완료했습니다.");
                Thread.Sleep(1000);
            }
        }

    }
}

