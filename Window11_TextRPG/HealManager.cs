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
            MoveHealScene();
        }


        // Scene
        public void MoveHealScene()
        {
            DisplayManager.HealScene(player, potion);
            int input = UtilManager.PlayerInput(0, 1);

            switch (input)
            {
                case 0:
                    GameManager.Instance.ChangeScene(SceneState.LobbyManager);
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
    }
}
