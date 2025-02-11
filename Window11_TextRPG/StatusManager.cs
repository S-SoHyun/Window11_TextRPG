using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    internal class StatusManager : IScene
    {
        private static StatusManager instance;
        public static StatusManager Instance
        {
            get
            {
                if(instance == null)
                    instance = new StatusManager();
                return instance;
            }
        }


        public void Enter()
        {
            DisplayManager.StatusScene(PlayerManager.Instance._Player);

            string t = Console.ReadLine();
            if (t == "0")
            {
                GameManager.Instance.ChangeScene(SceneState.LobbyManager);
            }
            else StatusManager.Instance.Enter();
            
        }
    }
}
