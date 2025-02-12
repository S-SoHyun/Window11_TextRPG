using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{


  public class PlayerManager : IScene
  {

    private PlayerManager()
    {
      player = new Player();
    }
    private static PlayerManager? instance;
    public static PlayerManager Instance
    {
      get
      {
        if (instance == null)
          instance = new PlayerManager();
        return instance;
      }
    }

    Player player;

    public Player _Player
    {


      get { return player; }
      set { player = value; }

    }

    public void Enter()
    {
      DisplayManager.ChooseNameScene(player);
      string input = Console.ReadLine().ToString();
      DisplayManager.ChooseJobScene(player);

      //직업정하기

      int result = UtilManager.PlayerInput(1, 4);
      switch (result)
      {
        case 1:
          player = new Player("전사", input, 130, 10);
          break;

        case 2:
          player = new Player("마법사", input, 90, 17);
          break;

        case 3:
          player = new Player("도적", input, 100, 15);
          break;

        case 4:
          player = new Player("궁수", input, 80, 20);
          break;
        default:
          break;
      }
      Console.WriteLine(player.name);
      Console.WriteLine(player.job);

      GameManager.Instance.ChangeScene(SceneState.LobbyManager);


    }
  }
}

