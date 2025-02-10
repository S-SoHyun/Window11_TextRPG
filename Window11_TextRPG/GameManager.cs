using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    enum SceneState
    {
        LobbyManager,
        PlayerManager,
        DungeonManager,
        InventoryManager,
        StoreManager
    }

    internal class GameManager
    {
        // Iscene 리스트
        private IScene[] ISceneList;
        private IScene currScene;      // 현재 씬 저장용

        // 싱글톤
        private static GameManager? instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        // 생성자
        public GameManager() 
        {
            // Scene 리스트 초기화
            ISceneList = new IScene[Enum.GetNames(typeof(SceneState)).Length];

            ISceneList[(int)SceneState.LobbyManager]        = LobbyManager.Instance;
            ISceneList[(int)SceneState.PlayerManager]       = PlayerManager.Instance;  
            ISceneList[(int)SceneState.DungeonManager]      = DungeonManager.Instance;
            ISceneList[(int)SceneState.InventoryManager]    = InventoryManager.Instance;
            ISceneList[(int)SceneState.StoreManager]        = StoreManager.Instance;
            
        }

        // 씬 (manager) 변화 
        public void ChangeScene(SceneState _state) 
        {
            currScene = ISceneList[(int)_state];

            if (currScene != null)
            {
                // enter 실행 
                currScene.Enter();
            }
        }

        public void GetSave() 
        {
            Console.WriteLine("GameManager의 Save 함수입니다.");
        }

        // 임시변수 (추후 삭제)
        List<MountableItem> mountableItems;
        List<PotionItem> potions;

        // 프로젝트 경로
        static string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        // 게임 저장 메서드
        public void SaveGame(Player player, List<MountableItem> mountableItems, List<PotionItem> potionItem)
        {
            try { SaveCharacter(player); }
            catch { Failed("캐릭터 저장"); }

            try { SaveItems(mountableItems, "MountableItem"); }
            catch { Failed("MountableItem 저장"); }

            try { SaveItems(potionItem, "PotionItem"); }
            catch { Failed("PotionItem 저장"); }
        }
        // 게임 불러오기 메서드
        public void LoadGame(Player player, List<MountableItem> mountableItems, List<PotionItem> potionItem)
        {
            try { LoadCharacter(ref player); }
            catch { Failed("캐릭터 불러오기"); }

            try { LoadItems(ref mountableItems, "MountableItem"); }
            catch { Failed("MountableItem 불러오기"); }

            try { LoadItems(ref potionItem, "PotionItem"); }
            catch { Failed("PotionItem 불러오기"); }
        }

        //============게임 저장 관련 메서드===============
        void SaveCharacter(Player player)
        {
            // 캐릭터 저장
            string filePath = Path.Combine(projectDir, "data", "character.json");
            string jsn = JsonSerializer.Serialize(player
                        , new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            File.WriteAllText(filePath, jsn);
            Console.WriteLine("캐릭터 저장 완료!");
            Console.WriteLine(filePath);
        }
        void SaveItems(List<MountableItem> _items, string _str)
        {
            // 아이템 저장(MountableItem)
            string filePath = Path.Combine(projectDir, "data", $"{_str}.json");
            string json = JsonSerializer.Serialize(_items
                , new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            File.WriteAllText(filePath, json);
            Console.WriteLine("아이템 저장 완료!");
            Console.WriteLine(filePath);
        }
        void SaveItems(List<PotionItem> _items, string _str)
        {
            // 아이템 저장(PotionItem)
            string filePath = Path.Combine(projectDir, "data", $"{_str}.json");
            string json = JsonSerializer.Serialize(_items
                , new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            File.WriteAllText(filePath, json);
            Console.WriteLine("아이템 저장 완료!");
            Console.WriteLine(filePath);
        }
        //============게임 로드 관련 메서드===============
        void LoadCharacter(ref Player player)
        {
            // 캐릭터 로드
            string filePath = Path.Combine(projectDir, "data", "character.json");
            string jsn = File.ReadAllText(filePath);

            player = JsonSerializer.Deserialize<Player>(jsn);

            Console.WriteLine("캐릭터 불러오기 완료!");
            Console.WriteLine(filePath);
        }
        void LoadItems(ref List<MountableItem> _items, string _str)
        {
            // 아이템 로드(MountableItem)
            string filePath = Path.Combine(projectDir, "data", $"{_str}.json");
            string jsn = File.ReadAllText(filePath);

            _items = JsonSerializer.Deserialize<List<MountableItem>>(jsn);

            Console.WriteLine("아이템 불러오기 완료!");
            Console.WriteLine(filePath);
        }
        void LoadItems(ref List<PotionItem> _items, string _str)
        {
            // 아이템 로드(PotionItem)
            string filePath = Path.Combine(projectDir, "data", $"{_str}.json");
            string jsn = File.ReadAllText(filePath);

            _items = JsonSerializer.Deserialize<List<PotionItem>>(jsn);

            Console.WriteLine("아이템 불러오기 완료!");
            Console.WriteLine(filePath);
        }
        void Failed(string str)
        {
            Console.WriteLine($"{str} 실패");
        }

    }
}
