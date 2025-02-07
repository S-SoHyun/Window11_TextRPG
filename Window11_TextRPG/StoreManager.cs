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

        // GameManager에서 접근하는 곳
        public void Enter()
        {
            while (true)
            {
                // DiplayManager 접근

                int result = UtilManager.PlayerInput(1, 3);
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

        public void BuyItemScene()
        {
            // DiplayManager 접근
            int result = UtilManager.PlayerInput(1, 3);
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

        public void SellItemScene()
        {
            // DiplayManager 접근
        }

        private void BuyItem(int idx)
        {
            Character character = Character.GetInst();
            Core core = Core.GetInst();

            // 구매 불가
            if (core.items[idx - 1].Own)
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                Thread.Sleep(1000);
            }
            // 구매 가능
            else
            {
                // Gold 충분
                if (core.items[idx - 1].Gold <= character.Gold)
                {
                    core.items[idx - 1].Own = true;
                    character.Gold -= core.items[idx - 1].Gold;
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
            Core core = Core.GetInst();
            Character character = Character.GetInst();

            EquipmentScene equipmentScene = EquipmentScene.GetInst();
            Item item = core.items[idx - 1];
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
                    equipmentScene.MountItem(item, false);
                }
                item.Own = false;
                character.Gold += (int)((float)item.Gold * 0.85f);
                Console.WriteLine("판매를 완료했습니다.");
                Thread.Sleep(1000);
            }
        }

    }
}

