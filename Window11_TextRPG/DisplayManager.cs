using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    internal static class DisplayManager
    {
        // 모든 Scene에서 화면을 출력할 때 사용되는 메니저

        private static void Clear()
        {
            Console.Clear();
        }

        private static void AddBlankLine(int count = 1)
        {
            // 빈줄 추가 메서드
            // count 수치 만큼 빈줄 추가
            for (int i = 1; i < count; i++)
            {
                Console.WriteLine("");
            }
        }


        public static void StatusScene(Player player)
        {
            Clear();
            Console.WriteLine("[상태보기]");
            AddBlankLine(2);

            Console.WriteLine("이름: " + player.name);
            Console.WriteLine("Lv." + player.level);
            Console.WriteLine("직업: " + player.job);
            Console.WriteLine("공격력: " + player.atk);
            Console.WriteLine("방어력: " + player.def);
            Console.WriteLine("체력: " + player.hp);
            Console.WriteLine("Gold: " + player.gold);

            AddBlankLine(2);
            Console.WriteLine("0. 나가기");

            AddBlankLine();
            Console.WriteLine("원하시는 행동을 입력해주세요");
            Console.Write(">>> ");
        }

        public static void MainScene()
        {

        }

        public static void DungeonScene(Player player, Monster monster)
        {

        }

        public static void DungeonPlayerAttackScene(Player player, Monster monster)
        {

        }

        public static void DungeonMonsterAttackScene(Player player, Monster monster)
        {

        }

        public static void DungeonWinResultScene(Player player, Monster monster)
        {

        }

        public static void DungeonLoseResultScene(Player player, Monster monster)
        {

        }

        public static void InventoryScene(Player plyaer, List<Item> items)
        {

        }

        public static void EquipmentScene(Player plyaer, List<Item> items)
        {

        }

        public static void StoreScene(Player plyaer, List<Item> items)
        {

        }

        public static void StoreBuyScene(Player plyaer, List<Item> items)
        {

        }

        public static void StoreSellScene(Player plyaer, List<Item> items)
        {

        }
    }
}
