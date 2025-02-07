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
        public static StoreManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new StoreManager();
                return instance;
            }
        }
        // 인벤토리에서 가져온 아이템 리스트
        List<MountableItem> items = InventoryManager.Instance.mountableItems;

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
            // 다음 Scene 지정
            Action nextScene = StoreScene;

            bool choose = false;
            while (!choose)
            {
                // StoreScene 출력
                DisplayManager.StoreScene(player, items);
                int result = UtilManager.PlayerInput(0, 2);

                // 나가기 버튼
                if (0 == result)
                {
                    Console.WriteLine("debug로비로 간다");
                    Thread.Sleep(1000);
                    nextScene = StoreScene;
                    choose = true;
                }

                switch (result)
                {
                    case 1: // 구매 페이지 진입
                        nextScene = BuyItemScene;
                        choose = true;
                        break;

                    case 2: // 판매 페이지 진입
                        nextScene = SellItemScene;
                        choose = true;
                        break;
                }
            }
            nextScene();
        }

        private void BuyItemScene()
        {
            // 다음 Scene 지정
            Action nextScene = BuyItemScene;

            bool choose = false;
            while (!choose)
            {
                // DiplayManager 접근
                DisplayManager.StoreBuyScene(player, items);
                int result = UtilManager.PlayerInput(0, items.Count() + 1);

                // 나가기 버튼
                if (0 == result)
                {
                    nextScene = StoreScene;
                    choose = true;
                }
                // 아이템 구매 접근
                else
                {
                    BuyItem(result);
                }
            }
            nextScene();
        }

        private void SellItemScene()
        {
            // 다음 Scene 지정
            Action nextScene = SellItemScene;

            bool choose = false;
            while (!choose)
            {
                // DiplayManager 접근
                DisplayManager.StoreSellScene(player, items);

                int result = UtilManager.PlayerInput(0, items.Count() + 1);

                // 나가기 버튼
                if (0 == result)
                {
                    nextScene = StoreScene;
                    choose = true;
                }
                // 아이템 팬매 접근
                else
                {
                    SellItem(result);
                }

            }
            nextScene();
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

