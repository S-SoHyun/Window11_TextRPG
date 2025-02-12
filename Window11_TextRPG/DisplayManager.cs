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
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("");
            }
        }

        private static void InputInduction()
        {
            AddBlankLine();
            Console.WriteLine("원하시는 행동을 입력해주세요");
            // Console.Write(">>> ");
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

            InputInduction();
        }

        public static void MainScene()
        {
            Console.WriteLine("RPG 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");

            AddBlankLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 들어가기");
            Console.WriteLine("5. 휴식");

            AddBlankLine(2);
            Console.WriteLine("6. 저장하기");
            Console.WriteLine("7. 불러오기");

            AddBlankLine(2);
            Console.WriteLine("0. 게임 종료");

            InputInduction();
        }
        public static void ChooseNameScene(Player player)
        {
            Clear();
            AddBlankLine(2);

            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정해주세요\n>>");
            

        }
        public static void ChooseJobScene(Player player)
        {
            Clear();
            Console.WriteLine("[직업선택]");
            AddBlankLine(2);

            Console.WriteLine("1.전사 HP:130  공격력:10");
            Console.WriteLine("2.마법사 HP:90  공격력:10");
            Console.WriteLine("3.도적 HP:100  공격력:15");
            Console.WriteLine("4.궁수 HP:80  공격력:20");
            AddBlankLine(2);



            InputInduction();
        }

        public static void DungeonScene(Player player, List<Monster> monsters)
        {
            Clear();
            Console.WriteLine("Battle!!");
            AddBlankLine();
            for (int i = 0; i < monsters.Count; i++)
            {
                Console.WriteLine($"Lv.{monsters[i].level} {monsters[i].name} {(monsters[i].hp <= 0 ? "Dead" : "HP " + monsters[i].hp)}");
            }
            AddBlankLine(2);
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.level} {player.name} ({player.job})");
            Console.WriteLine($"HP {player.hp}/{player.maxhp}");
            AddBlankLine();
            Console.WriteLine("1. 공격");
            AddBlankLine();
            InputInduction();
        }

        public static void DungeonMonsterTargetScene(Player player, List<Monster> monsters)
        {
            Clear();
            Console.WriteLine("Battle!!");
            AddBlankLine();
            for (int i = 0; i < monsters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Lv.{monsters[i].level} {monsters[i].name} {(monsters[i].hp <= 0 ? "Dead" : "HP " + monsters[i].hp)}");
            }
            AddBlankLine(2);
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.level} {player.name} ({player.job})");
            Console.WriteLine($"HP {player.hp}/{player.maxhp}");
            AddBlankLine();
            Console.WriteLine("0. 취소");
            AddBlankLine();
            InputInduction();
        }

        public static void DungeonPlayerAttackScene(Player player, Monster monster, int playerDamage, int beforeMonsterHp,int hitType)
        {
            Clear();
            Console.WriteLine("Battle!!");
            AddBlankLine();
            Console.WriteLine($"{player.name} 의 공격!");
            switch (hitType)
            {
                case 1:
                    Console.WriteLine($"Lv.{monster.level} {monster.name} 을(를) 공격했지만 아무일도 일어나지 않았습니다.");
                    break;
                case 2:
                    Console.WriteLine($"Lv.{monster.level} {monster.name} 을(를) 맞췄습니다!! [데미지 : {playerDamage}] -치명타 공격!!");
                    break;
                default:
                    Console.WriteLine($"Lv.{monster.level} {monster.name} 을(를) 맞췄습니다. [데미지 : {playerDamage}]");
                    break;
            }
            AddBlankLine();
            Console.WriteLine($"Lv.{monster.level} {monster.name}");
            Console.WriteLine($"HP {beforeMonsterHp} -> {(monster.IsDie() ? "Dead" : beforeMonsterHp - playerDamage)}");
            AddBlankLine();
            Console.WriteLine("0. 다음");
            AddBlankLine();
            Console.Write(">> ");
        }

        public static void DungeonMonsterAttackScene(Player player, Monster monster, int beforePlayerHp)
        {
            Clear();
            Console.WriteLine("Battle!!");
            AddBlankLine();
            Console.WriteLine($"{monster.name} 의 공격!");
            Console.WriteLine($"{player.name} 을(를) 맞췄습니다. [데미지 : {monster.Attack()}]");
            AddBlankLine();
            Console.WriteLine($"Lv.{player.level} {player.name}");
            Console.WriteLine($"HP {beforePlayerHp} -> {player.hp}");
            AddBlankLine();
            Console.WriteLine("0. 다음");
            AddBlankLine();
            Console.Write(">> ");
        }

        public static void DungeonWinResultScene(Player player, int monsterCount, int playerHpBeforeEnter, int gold, int potionCount,MountableItem? item, int gainedExp, int expBeforeGain, int expForNextLevel, bool leveledUp)
        {
            Clear();
            Console.WriteLine("Battle!! - Result");
            AddBlankLine();
            Console.WriteLine("Victory");
            AddBlankLine();
            Console.WriteLine($"던전에서 몬스터 {monsterCount}마리를 잡았습니다.");
            AddBlankLine();
            Console.WriteLine("[캐릭터 정보]");
            Console.WriteLine($"Lv.{player.level} {player.name}");
            Console.WriteLine($"HP {playerHpBeforeEnter} -> {player.hp}");
            AddBlankLine();

            Console.WriteLine($"[경험치 획득] {gainedExp} EXP 획득!");

            if (leveledUp)
            {
                Console.WriteLine($"레벨업! Lv.{player.level} 으로 상승!");
            }
            else
            {
                int expNeeded = expForNextLevel - player.exp;
                Console.WriteLine($"다음 레벨까지 {expNeeded} EXP 필요");
            }

            if (gold != 0 || potionCount != 0 || item != null)
            {
                AddBlankLine();
                Console.WriteLine("[획득 아이템]");
                if (gold != 0)
                {
                    Console.WriteLine($"{gold} Gold");
                }
                if (potionCount != 0)
                {
                    Console.WriteLine($"포션 - {potionCount}");
                }
                if (item != null)
                {
                    Console.WriteLine($"{item.Name} - 1");
                }
            }
            AddBlankLine();
            Console.WriteLine("0. 다음");
            AddBlankLine();
            Console.Write(">> ");
        }

        public static void DungeonLoseResultScene(Player player, int playerHpBeforeEnter)
        {
            Clear();
            Console.WriteLine("Battle!! - Result");
            AddBlankLine();
            Console.WriteLine("You Lose");
            AddBlankLine();
            Console.WriteLine($"Lv.{player.level} {player.name}");
            Console.WriteLine($"HP {playerHpBeforeEnter} -> 0");
            AddBlankLine();
            Console.WriteLine("0. 다음");
            AddBlankLine();
            Console.Write(">> ");
        }

        public static void InventoryScene(Player player, List<MountableItem> items)
        {
            Clear();
            Console.Write("[인벤토리]");

            AddBlankLine();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            AddBlankLine();
            Console.WriteLine("[아이템 목록]");

            AddBlankLine();
            ViewInventoryItem(items);

            AddBlankLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");

            AddBlankLine();

            InputInduction();
        }

        public static void EquipmentScene(Player player, List<MountableItem> items)
        {
            Clear();
            Console.Write("[인벤토리- 장착 관리]");

            AddBlankLine();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            AddBlankLine();
            Console.WriteLine("[아이템 목록]");

            AddBlankLine();
            ViewEquipItem(items);

            AddBlankLine();
            Console.WriteLine("0. 나가기");

            InputInduction();
        }

        // 인벤토리 창에서 보일 아이템 목록
        public static void ViewInventoryItem(List<MountableItem> items)     
        {

            for (int i = 0; i < items.Count; i++)
            {
                string typeName = (items[i].Attack > 0) ? "공격력" : "방어력";
                int value = (items[i].Attack > 0) ? items[i].Attack : items[i].Defense;

                if (items[i].Equip)
                {
                    Console.Write("- [E]");
                }
                else
                {
                    Console.Write("- ");
                }
                Console.WriteLine($"{items[i].Name} | {typeName} + {value} |  {items[i].Description}");
            }
        }

        // 장착관리 창에서 보일 아이템 목록
        public static void ViewEquipItem(List<MountableItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                string typeName = (items[i].Attack > 0) ? "공격력" : "방어력";
                int value = (items[i].Attack > 0) ? items[i].Attack : items[i].Defense;

                if (items[i].Equip)
                {
                    Console.Write($"- {i + 1} [E]");
                }
                else
                {
                    Console.Write($"- {i + 1} ");
                }
                Console.WriteLine($"{items[i].Name} | {typeName} + {value} | {items[i].Description}");
            }
        }


        public static void StoreScene(Player plyaer, List<MountableItem> items)
        {
            Clear();
            Console.Write("[상점]");

            AddBlankLine();
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");

            AddBlankLine();
            Console.WriteLine("[보유골드]");
            Console.WriteLine($"{plyaer.gold} G");

            AddBlankLine();
            Console.WriteLine("[아이템 목록]");

            printStoreItem(items, false);

            AddBlankLine(2);
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");

            AddBlankLine();
            Console.WriteLine("0. 나가기");

            InputInduction();
        }

        public static void StoreBuyScene(Player plyaer, List<MountableItem> items)
        {
            Clear();
            Console.Write("[상점 - 구매]");

            AddBlankLine();
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");

            AddBlankLine();
            Console.WriteLine("[보유골드]");
            Console.WriteLine($"{plyaer.gold} G");

            AddBlankLine();
            Console.WriteLine("[아이템 목록]");

            printStoreItem(items, false);

            AddBlankLine(2);
            Console.WriteLine("0. 나가기");

            InputInduction();
        }

        public static void StoreSellScene(Player plyaer, List<MountableItem> items)
        {
            Clear();
            Console.Write("[상점 - 판매]");

            AddBlankLine();
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");

            AddBlankLine();
            Console.WriteLine("[보유골드]");
            Console.WriteLine($"{plyaer.gold} G");

            AddBlankLine();
            Console.WriteLine("[아이템 목록]");

            printStoreItem(items, true);

            AddBlankLine(2);
            Console.WriteLine("0. 나가기");

            InputInduction();
        }

        // fasle: buy, true: sell
        private static void printStoreItem(List<MountableItem> items, bool buy_sell)
        {
            // item 출력
            int count = 0;
            // 장비 장착 옵션
            string equip = "구매완료";
            string AorD = "";
            foreach (MountableItem item in items)
            {
                count++;
                equip = "구매완료";
                // 공격력 or 방어력
                AorD = "";

                // 방어 장비
                if (item.Defense > 0)
                {
                    AorD += $"방어력 +{item.Defense} | ";
                }
                // 공격 장비
                if (item.Attack > 0)
                {
                    AorD += $"공격력 +{item.Attack} | ";
                }

                // 구매 리스트
                if (!buy_sell)
                {
                    // 소유한 아이템
                    if (item.Own)
                    {
                        Console.WriteLine($" -{count} {item.Name} | {AorD}{item.Description} | {equip}");
                    }
                    // 미소유 아이템
                    else
                    {
                        Console.WriteLine($" -{count} {item.Name} | {AorD}{item.Description} | {item.Price} G");
                    }
                }
                // 판매 리스트
                else
                {
                    equip = "미소유";
                    // 미소유 아이템
                    if (!item.Own)
                    {
                        Console.WriteLine($" -{count} {item.Name} | {AorD} | {item.Description} | {equip}");
                    }
                    // 소유한 아이템
                    else
                    {
                        Console.WriteLine($" -{count} {item.Name} | {AorD} | {item.Description} | {(int)((float)item.Price * 0.85f)} G");
                    }
                }
            }
        }

        // DisplayerManger에 함수 옮기기!
        public static void PrintMenu(string[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                Console.WriteLine($"{i + 1} . {list[i]}");
            }
        }

    }
}
