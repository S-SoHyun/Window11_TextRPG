using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        // 생성자
        public GameManager() 
        {
            // Scene 리스트 초기화
            ISceneList = new IScene[Enum.GetNames(typeof(SceneState)).Length];

            /*
            ISceneList[(int)SceneState.LobbyManager] = new LobbyManager();
            ISceneList[(int)SceneState.PlayerManager] = new PlayerManager();
            ISceneList[(int)SceneState.DungeonManager] = new DungeonManager();

            ISceneList[(int)SceneState.InventoryManager] = new InventoryManager();
            ISceneList[(int)SceneState.StoreManager] = new StoreManager();
            */
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

    }
}
