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

        // Player player = PlayerManager.Instance._Player;

        // GameManager에서 접근하는 곳
        public void Enter()
        {
            // StoreScene 진입
            StoreScene();
        }

        private void StoreScene()
        {
            Player player = PlayerManager.Instance._Player;
            // 다음 Scene 지정
            Action nextScene = StoreScene;
            int result = -1;

            // StoreScene 출력
            DisplayManager.StoreScene(player, items);
            result = UtilManager.PlayerInput(0, 2);

            switch (result)
            {
                case 0: // 로비
                    GameManager.Instance.ChangeScene(SceneState.LobbyManager);
                    return;

                case 1: // 구매 페이지
                    nextScene = BuyItemScene;
                    break;

                case 2: // 판매 페이지
                    nextScene = SellItemScene;
                    break;
            }

            nextScene();
        }

        private void BuyItemScene()
        {
            Player player = PlayerManager.Instance._Player;
            // 다음 Scene 지정
            Action nextScene = BuyItemScene;

            int result = -1;
            int inputCount = items.Count();

            // DiplayManager 접근
            DisplayManager.StoreBuyScene(player, items);
                
            result = UtilManager.PlayerInput(0, inputCount);

            // 나가기 버튼
            if (0 == result)
            {
                nextScene = StoreScene;
            }
            // 아이템 구매 접근
            else
            {
                BuyItem(result);
            }

            nextScene();
        }

        private void SellItemScene()
        {
            Player player = PlayerManager.Instance._Player;
            // 다음 Scene 지정
            Action nextScene = SellItemScene;

            int result = -1;
            int inputCount = items.Count();

            // DiplayManager 접근
            DisplayManager.StoreBuyScene(player, items);

            result = UtilManager.PlayerInput(0, inputCount);

            // 나가기 버튼
            if (0 == result)
            {
                nextScene = StoreScene;
            }
            // 아이템 판매 접근
            else
            {
                SellItem(result);
            }

            nextScene();
        }

        private void BuyItem(int idx)
        {
            Player player = PlayerManager.Instance._Player;
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
            Player player = PlayerManager.Instance._Player;
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
                    InventoryManager.Instance.Unequip(item);
                }
                player.gold += (int)((float)item.Price * 0.85f);
                Console.WriteLine("판매를 완료했습니다.");
                Thread.Sleep(1000);
            }
        }

    }
}

