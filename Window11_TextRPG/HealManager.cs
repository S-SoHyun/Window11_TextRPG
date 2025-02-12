using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;
using static System.Net.Mime.MediaTypeNames;

namespace Window11_TextRPG
{
    public class HealManager : IScene
    {
        // Singleton
        private HealManager() { }
        public static HealManager? instance;

        public static HealManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new HealManager();
                return instance;
            }
        }

        // 변수
        PotionItem potion = InventoryManager.Instance.potion;
        // player
        Player player = PlayerManager.Instance._Player;


        // 중앙 통제
        public void Enter()
        {
            MoveBeforeBattleScene();
        }


        // Scene
        public void MoveBeforeBattleScene()
        {
            //DisplayManager.BeforeBattleScene();                      // displayManager에 추가한 후 주석 없애기
            int input = UtilManager.PlayerInput(0, 3);

            switch (input)
            {
                case 0:
                    GameManager.Instance.ChangeScene(SceneState.LobbyManager);
                    break;
                case 1:
                    GameManager.Instance.ChangeScene(SceneState.StatusManager);
                    break;
                case 2:
                    GameManager.Instance.ChangeScene(SceneState.DungeonManager);
                    break;
                case 3:
                    MoveHealScene();
                    break;
            }
        }


        // Scene
        public void MoveHealScene()
        {
            //DisplayManager.HealScene(player, potion);             // displayManager에 추가한 후 주석 없애기
            int input = UtilManager.PlayerInput(0, 1);

            switch (input)
            {
                case 0:
                    MoveBeforeBattleScene();
                    break;
                case 1:    // 사용하기
                    UsePotion(player, potion);
                    UtilManager.DelayForSecond(1);
                    MoveHealScene();
                    break;
            }
        }


        public void UsePotion(Player player, PotionItem potion)
        {
            if (potion.Count > 0)
            {
                int health = UtilManager.CalcPlayerHp(player, potion.Heel);
                if (player.hp == player.maxhp)
                {
                    Console.WriteLine("이미 최대 체력입니다.");
                }
                else
                {
                    potion.Count -= 1;
                    Console.WriteLine("회복을 완료했습니다.");
                }
            }
            else
            {
                Console.WriteLine("포션이 부족합니다.");
            }
        }



        // ↓ DisplayManager에 넣을 것 ↓ //

        //public static void BeforeBattleScene()
        //{
        //    Clear();
        //    Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
        //    Console.WriteLine("이제 전투를 시작할 수 있습니다.");

        //    AddBlankLine();
        //    Console.WriteLine("1. 상태 보기");
        //    Console.WriteLine("2. 전투 시작");
        //    Console.WriteLine("3. 회복 아이템");

        //    AddBlankLine();
        //    Console.WriteLine("0. 나가기");

        //    AddBlankLine();

        //    InputInduction();
        //}


        //public static void HealScene(Player player, PotionItem potion)
        //{
        //    Clear();
        //    Console.WriteLine("[회복]");
        //    Console.WriteLine("포션을 사용하면 체력을 30 회복할 수 있습니다.");
        //    Console.WriteLine($"( 현재 체력 : {player.hp} )");
        //    Console.WriteLine($"( 남은 포션 : {potion.Count} )");

        //    AddBlankLine();
        //    Console.WriteLine("1. 사용하기");
        //    Console.WriteLine("0. 나가기");

        //    AddBlankLine();

        //    InputInduction();
        //}
    }
}
