using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    internal class LobbyManager : IScene
    {
        private string[] lobbyList;

        // 생성자
        private LobbyManager() 
        {
            lobbyList = new string[6]
            {
                "상태 보기",
                "전투 시작",
                "인벤토리",
                "던전",
                "퀘스트",
                "저장하기",
            };
        }
        // 싱글톤 
        private static LobbyManager? instance;
        public static LobbyManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LobbyManager();
                return instance;
            }
        }

        public void Enter()
        {
            // 출력
            DisplayManager.PrintMenu(lobbyList);

            // player input 
            int input = UtilManager.PlayerInput(1, lobbyList.Length);

            switch (input) 
            {
                case 1:
                    // 플레이어 Manger로 변환

                    break;
                case 2:
                    // 던전 Manger로 변환

                    break;
                case 3:
                    // 인벤토리 Manger로 변환

                    break;
                case 4:
                    // 던전 Manger로 변환

                    break;
                case 5:
                    // 퀘스트 Manger로 변환

                    break;
                case 6:
                    // 저장하기
                    GameManager.Instance.GetSave();
                    break;
                default:
                    // 다시 로비로 돌아가기

                    break;
            }

        }


    }
}
