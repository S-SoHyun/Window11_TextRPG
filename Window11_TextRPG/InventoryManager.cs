using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Window11_TextRPG;

namespace Window_11_TEXTRPG
{
    public class InventoryManager : IScene
    {
        // Singleton
        private InventoryManager()
        {
            reward = new Reward(mountableItems, potion);
        }
        public static InventoryManager? instance;

        public static InventoryManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InventoryManager();
                return instance;
            }
        }

        // reward
        private Reward reward;
        public Reward RewardInstnace => reward;


        // temp. 완성될 땐 지우기
        Player player = new Player();


        // 장착 가능 아이템 생성
        public List<MountableItem> mountableItems = new List<MountableItem>()
        {
             new MountableItem()
            {
                Name = "수련자의 갑옷",
                Description = "수련에 도움을 주는 갑옷입니다.",
                Price = 1000,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 4,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "무쇠갑옷",
                Description = "무쇠로 만들어져 튼튼한 갑옷입니다.",
                Price = 2000,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 9,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "스파르타의 갑옷",
                Description = "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.",
                Price = 3500,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 15,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "낡은 검",
                Description = "쉽게 볼 수 있는 낡은 검 입니다.",
                Price = 600,
                Type = ITEMTYPE.WEAPON,

                Attack = 5,
                Defense = 0,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "청동 도끼",
                Description = "어디선가 사용됐던거 같은 도끼입니다.",
                Price = 1500,
                Type = ITEMTYPE.WEAPON,

                Attack = 10,
                Defense = 0,
                Own = false,
                Equip = false
            },

            new MountableItem()
            {
                Name = "스파르타의 창",
                Description = "스파르타의 전사들이 사용했다는 전설의 창입니다.",
                Price = 2500,
                Type = ITEMTYPE.WEAPON,

                Attack = 20,
                Defense = 0,
                Own = false,
                Equip = false
            },
        };


        // 포션 생성
        public PotionItem potion = new PotionItem()
        {
            Name = "HP 포션",
            Description = "HP를 30 회복할 수 있는 포션입니다.",
            Price = 500,
            Type = ITEMTYPE.POTION,

            Count = 3,
            Heel = 30
        };

        

        // 상점에서 산 아이템 리스트 생성
        public List<MountableItem> ownItems = new List<MountableItem>();


        // 인벤토리 매니저에서만 사용. 상점에서 샀고 타입이 웨폰인 리스트 / 상점에서 샀고 타입이 아머인 리스트 세분화하여 생성
        List<MountableItem> ownWeapons = new List<MountableItem>();
        List<MountableItem> ownArmors = new List<MountableItem>();


        // 중앙 통제
        public void Enter()
        {
            MoveInventoryScene();
        }


        // 인벤토리 창으로 이동
        public void MoveInventoryScene()
        {
            DivisionOwnItem(mountableItems);
            DivisionType(ownItems);
            ownItems = ownItems.Distinct().ToList();    // 목록 중복되는 문제 distinct로 제거

            DisplayManager.InventoryScene(player, ownItems);
            int input = UtilManager.PlayerInput(0, 1);

            switch (input)
            {
                case 0:
                    GameManager.Instance.ChangeScene(SceneState.LobbyManager);
                    break;
                case 1:
                    MoveEquipmentScene(ownItems);
                    break;
            }
        }


        // 장착 관리 창으로 이동
        public void MoveEquipmentScene(List<MountableItem> items)
        {
            items = items.Distinct().ToList();
            DisplayManager.EquipmentScene(player, items);
            int input = UtilManager.PlayerInput(0, items.Count);

            if (input == 0)
            {
                MoveInventoryScene();
            }
            else if (input > 0 && input <= items.Count)
            {
                Equip(input);
                MoveEquipmentScene(items);
            }
        }


        // 상점에서 산 아이템들 따로 리스트에 넣기
        public void DivisionOwnItem(List<MountableItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Own)
                    ownItems.Add(items[i]);
            }
        }


        // 상점에서 산 아이템들 중에서 타입에 따라 또 나누기 (웨폰 / 아머)
        public void DivisionType(List<MountableItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Type == ITEMTYPE.WEAPON)
                {
                    ownWeapons.Add(items[i]);
                }
                else if (items[i].Type == ITEMTYPE.ARMOR)
                {
                    ownArmors.Add(items[i]);
                }
            }
        }


        // 장착 (타입에 따라 중복 장착이 안 되도록)
        public void Equip(int input)
        {
            MountableItem select = ownItems[input - 1];
            MountableItem equipped = null;

            
            if (select.Type == ITEMTYPE.WEAPON)                     // < 고른 게 웨폰이라면 장착된 게 있는지 확인 후 장착>  
            {
                for (int i = 0; i < ownWeapons.Count; i++)          // weapon만 있는 리스트에서 equip이 true인 것 찾기
                {
                    if (ownWeapons[i].Equip)
                    {
                        equipped = ownWeapons[i];
                        break;
                    }
                }

                if (equipped != null && equipped == select)         // 1. 장착 아이템이 있는데 이게 선택한 아이템과 같다면
                {
                    Unequip(equipped);
                }
                else if (equipped != null && equipped != select)    // 2. 장착 아이템이 있는데 이게 선택한 아이템과 다르다면
                {
                    Unequip(equipped);
                    select.Equip = true;
                    player.atk += select.Attack;
                }
                else                                                // 3. 장착된 게 없다면
                {
                    select.Equip = true;
                    player.atk += select.Attack;
                }
            }
            else if (select.Type == ITEMTYPE.ARMOR)                 // < 고른 게 아머라면 장착된 게 있는지 확인 후 장착 > 
            {
                for (int i = 0; i < ownArmors.Count; i++)           // 아머만 있는 리스트에서 equip이 true인 것 찾기
                {
                    if (ownArmors[i].Equip)
                    {
                        equipped = ownArmors[i];
                        break;
                    }
                }

                if (equipped != null && equipped == select)         // 1. 장착 아이템이 있는데 이게 선택한 아이템과 같다면
                {
                    Unequip(equipped);
                }
                else if (equipped != null && equipped != select)    // 2. 장착 아이템이 있는데 이게 선택한 아이템과 다르다면
                {
                    Unequip(equipped);
                    select.Equip = true;
                    player.def += select.Defense;
                }
                else                                                // 3. 장착된 게 없다면
                {
                    select.Equip = true;
                    player.def += select.Defense;
                }
            }
        }


        // 장착 해제
        public void Unequip(MountableItem equipped)
        {
            equipped.Equip = false;
            if (equipped.Type == ITEMTYPE.WEAPON)
            {
                player.atk -= equipped.Attack;
            }
            else if (equipped.Type == ITEMTYPE.ARMOR)
            {
                player.def -= equipped.Defense;
            }
        }


        // 물약 사용
        public void UsePotion(PotionItem potion)
        {
            if (potion.Count > 0)      // 만약 물약이 한 개 이상 있다면
            {
                potion.Count--;
                player.hp += potion.Heel;
                Console.WriteLine("회복을 완료했습니다.");
            }
            else
                Console.WriteLine("포션이 부족합니다.");
        }
    }
}




