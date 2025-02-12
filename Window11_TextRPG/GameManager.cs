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
        StoreManager,
        QuestManager,
        StatusManager,
        HealManager
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
            ISceneList[(int)SceneState.QuestManager]        = QuestManager.Instance;
            ISceneList[(int)SceneState.StatusManager]       = StatusManager.Instance;
            ISceneList[(int)SceneState.HealManager]         = HealManager.Instance;
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


        public bool GetSave() 
        {
            Player player                           = PlayerManager.Instance._Player;
            List<MountableItem> mountableItems      = InventoryManager.Instance.mountableItems;
            PotionItem potion                       =  InventoryManager.Instance.potion;
            List<SaveQuestWrapper> saveQuestWrapper = QuestManager.Instance.SaveWrapper;

            for (int i = 0; i < saveQuestWrapper.Count; i++)
                Console.WriteLine(saveQuestWrapper[i].Name + " / " + saveQuestWrapper[i].QuestType);

            bool success = SaveGame(PlayerManager.Instance._Player, mountableItems, potion , saveQuestWrapper);
            UtilManager.DelayForSecond(2);
            ChangeScene(SceneState.LobbyManager);
            return success;
        }

        public bool GetLoad()
        {
            PotionItem potion = InventoryManager.Instance.potion;

            bool success = LoadGame(potion);
            UtilManager.DelayForSecond(2);
            ChangeScene(SceneState.LobbyManager);
            return success;
        }

        

        // 프로젝트 경로
        static string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        // 게임 저장 메서드
        public bool SaveGame(Player player, List<MountableItem> mountableItems, PotionItem potionItem , List<SaveQuestWrapper> saveQuestWrapper)
        {
            bool success = true;
            try { SaveCharacter(player); }
            catch { Failed("캐릭터 저장"); success = false; }

            try { SaveItems(mountableItems, "MountableItem"); }
            catch { Failed("MountableItem 저장"); success = false; }

            try { SaveItems(potionItem, "PotionItem"); }
            catch { Failed("PotionItem 저장"); success = false; }

            try { SaveQuestWrapper(saveQuestWrapper, "QuestWrapper"); }
            catch { Failed("PotionItem 저장"); success = false; }

            return success;
        }
        // 게임 불러오기 메서드
        public bool LoadGame(PotionItem potion)
        {
            bool success = true;
            try { LoadCharacter(); }
            catch { Failed("캐릭터 불러오기"); success = false; }

            try { LoadItems("MountableItem"); }
            catch { Failed("MountableItem 불러오기"); success = false; }

            try { LoadItems(potion, "PotionItem"); }
            catch { Failed("PotionItem 불러오기"); success = false; }

            return success;
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
        void SaveItems(PotionItem _items, string _str)
        {
            // 아이템 저장(PotionItem)
            string filePath = Path.Combine(projectDir, "data", $"{_str}.json");
            string json = JsonSerializer.Serialize(_items
                , new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            File.WriteAllText(filePath, json);
            Console.WriteLine("아이템 저장 완료!");
            Console.WriteLine(filePath);
        }

        void SaveQuestWrapper(List<SaveQuestWrapper> saveQuestWrapper, string str) 
        {
            // 퀘스트 Wrapper 저장(Quest)
            string filePath = Path.Combine(projectDir, "data", $"{str}.json");
            string json = JsonSerializer.Serialize(saveQuestWrapper
                , new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
            File.WriteAllText(filePath, json);
            Console.WriteLine("퀘스트 저장 완료!");
            Console.WriteLine(filePath);
        }

        //============게임 로드 관련 메서드===============
        void LoadCharacter()
        {
            // 캐릭터 로드
            string filePath = Path.Combine(projectDir, "data", "character.json");
            string jsn = File.ReadAllText(filePath);
            Player load = new Player();
            load = JsonSerializer.Deserialize<Player>(jsn);
            PlayerManager.Instance._Player = load;

            Console.WriteLine("캐릭터 불러오기 완료!");
            load.SetSkills(load.job);
            Console.WriteLine(filePath);
        }
        void LoadItems(string _str)
        {
            // 아이템 로드(MountableItem)
            string filePath = Path.Combine(projectDir, "data", $"{_str}.json");
            string jsn = File.ReadAllText(filePath);

            InventoryManager.Instance.mountableItems = JsonSerializer.Deserialize<List<MountableItem>>(jsn);

            Console.WriteLine("아이템 불러오기 완료!");
            Console.WriteLine(filePath);
        }
        void LoadItems(PotionItem item, string _str)
        {
            // 아이템 로드(PotionItem)
            string filePath = Path.Combine(projectDir, "data", $"{_str}.json");
            string jsn = File.ReadAllText(filePath);

            InventoryManager.Instance.potion = JsonSerializer.Deserialize<PotionItem>(jsn);

            Console.WriteLine("아이템 불러오기 완료!");
            Console.WriteLine(filePath);
        }
        void Failed(string str)
        {
            Console.WriteLine($"{str} 실패");
        }

    }
}
