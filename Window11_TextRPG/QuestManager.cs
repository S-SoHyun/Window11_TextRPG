using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;
using System.Threading;

namespace Window11_TextRPG
{
    public enum QuestState
    {
        beforeReceive,
        afterReceive,
        complete
    }


    internal class QuestManager : IScene
    {
        private List<Quest> quests;     // 퀘스트 컨테이너 
        private Quest currQuest;        // 현재 퀘스트 저장 
        private string[] questOption;     // 퀘스트 씬 옵션 목록
        private string[] rewardOptionByState;     // 스탯 별 출력해야할 옵션 : 수락 or 퀘받아가세요 or 보상받기

        // 퀘스트 수락 or 퀘 받기 or 보상받기 선택 시 실행할 메서드 저장해놓을 Action
        private Action actionOne;
        private Action actionTwo;

        // 생성자
        private QuestManager()
        {
            // 퀘스트 리스트 
            InitQuestList();

            // quest에서 이름만 빼서 배열로 저장 (LINQ)
            questOption = quests.Select(q => q.QuestName).ToArray();

            rewardOptionByState = new string[2];
        }
        // 싱글톤
        private static QuestManager? instance;
        public static QuestManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new QuestManager();
                return instance;
            }
        }

        public void Enter()
        {
            // 목록(리스트) 출력
            DisplayManager.PrintMenu(questOption);

            // player input 
            int input = UtilManager.PlayerInput(1, questOption.Length);

            // 현재 퀘스트 
            currQuest = quests[input - 1];

            // 퀘스트 print
            PrintQuest();

            // 성공여부에 따라 print 다름 
            PrintRewardBystate();
        }

        private void PrintQuest()
        {
            if (currQuest == null)
            {
                Console.WriteLine("QuestManager : Quest클래스 null 오류입니다");
                return;
            }

            Console.WriteLine(currQuest.QuestName + '\n');
            Console.WriteLine(currQuest.QuestStory + '\n');

            Console.WriteLine( "-" + currQuest.QuestProgress() + '\n');
            Console.WriteLine( "-" + currQuest.RewardItemAndGold() + '\n');
        }

        private void PrintRewardBystate() 
        {
            switch (currQuest.QuestState) 
            {
                case QuestState.beforeReceive:
                    rewardOptionByState[0] = "수락";
                    rewardOptionByState[1] = "거절";
                    actionOne += Acception;
                    actionTwo += DeclineQuest;
                    break;
                case QuestState.afterReceive:
                    rewardOptionByState[0] = "퀘스트를 완료하세요! ";
                    rewardOptionByState[1] = "돌아가기";
                    actionOne += CompleteAndComeBack;
                    actionTwo += ReturnToMenu;
                    break;
                case QuestState.complete:
                    rewardOptionByState[0] = "보상받기";
                    rewardOptionByState[1] = "돌아가기";
                    actionOne += Acception;
                    actionTwo += ReturnToMenu;
                    break;
            }

            // 목록(리스트) 출력
            DisplayManager.PrintMenu(rewardOptionByState);
        }

        private void Acception() 
        {
            // 퀘스트 수락 -> 현재 퀘스트를 accept 로 바꾸기
            currQuest.QuestState = QuestState.afterReceive;
        }
        private void DeclineQuest() 
        {
            Console.WriteLine("퀘스트를 거절하셨습니다 ");

            DelayForSecond("3초후 퀘스트 화면으로 돌아갑니다", 3);

            // 퀘스트 거절 -> 퀘스트씬으로 돌아가기 
            GameManager.Instance.ChangeScene(SceneState.QuestManager);
        }
        private void CompleteAndComeBack() 
        {
            // 퀘스트 완료필요 -> 완료하고 오세요 ! 
            Console.WriteLine("퀘스트를 완료하고 오세요!");

            DelayForSecond("3초후 퀘스트 화면으로 돌아갑니다", 3);

            // 퀘스트 메뉴로 돌아가기
            GameManager.Instance.ChangeScene(SceneState.QuestManager);
        }

        private void ReturnToMenu() 
        {
            DelayForSecond("3초후 퀘스트 화면으로 돌아갑니다", 3);

            // 퀘스트 메뉴로 돌아가기
            GameManager.Instance.ChangeScene(SceneState.QuestManager);
        }

        private void AcquirReward() 
        {
            Console.WriteLine("보상을 받았습니다. 인벤토리를 확인해주세요!");

            DelayForSecond("3초후 퀘스트 화면으로 돌아갑니다", 3);

            // 퀘스트 메뉴로 돌아가기
            GameManager.Instance.ChangeScene(SceneState.QuestManager);
        }

        // UTilManager로 옮기기
        public void DelayForSecond(string str, int second) 
        {
            Console.WriteLine(str);
            Thread.Sleep(second * 1000 );   // ex) 3 * 1000 : 3초 대기
        }

        private void InitQuestList()
        {
            quests = new List<Quest>();

            // 1번 : 미니언처치
            // -> Dungeon에서 Monster 생성 후 몇마리 처치 했는지 저장 필요함 
            // 2번 : armor 장착
            // 3번 : weapon 장착
            // -> InventoryManager안에 아이템list가 있는데 item에는 장착했는지 여부에 대한 enum 이 있어서
            // enum 기준으로 LINQ 한 다음에 배열이 0이상이면 있음 없음 정도로 해보면될듯

            #region 장비 리스트 초기화 
            quests.Add(new MonsterKillQuest
                (name: "마을을 위협하는 미니언 처치"
                , tooltip: "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\r\n" +
                    "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\r\n" +
                    "모험가인 자네가 좀 처치해주게!\r\n",
                perForm : "몬스터 처치하기",
                reward: 5,
                monstername: "Minion",
                killCount: 5));
            quests.Add(new EquiptQuest
                (name: "장비를 장착해보자"
                , tooltip: "모험가 자네 아직도 장비를 장착하지 않았는가?\r\n" +
                    "인벤토리에서 아무 장비나 장착하게! \r\n",
                perForm : "인벤토리에서 Armor 장비 장착하기",
                reward: 5,
                type : ITEMTYPE.ARMOR ,
                wearCnt : 2));
            quests.Add(new EquiptQuest
                (name: "더욱 더 강해지기!"
                , tooltip: "자네 맨손으로 싸우고있었나?\r\n" +
                    "무기를 끼면 강해진다네 \r\n" +
                    "인벤토리에서 무기를 착용하고 오게 \r\n",
                perForm: "인벤토리에서 Weapon 장비 장착하기",
                reward: 5,
                type: ITEMTYPE.WEAPON,
                wearCnt : 1));
            #endregion

            // 보상 아이템 세팅 ( ##TODO : 물약도 넣고싶은데 이건 물약넣는부분에서 수정후에  )
            quests[0].AddToItem(new MountableItem()
            {
                Name = "첫번째 퀘스트 옷",
                Description = "수련에 도움을 주는 갑옷입니다.",
                Price = 1000,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 4,
                Own = false,
                Equip = false
            }, 1);
            quests[1].AddToItem(new MountableItem()
            {
                Name = "두번째 퀘스트 옷",
                Description = "수련에 도움을 주는 갑옷입니다.",
                Price = 1000,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 4,
                Own = false,
                Equip = false
            }, 1); 
            quests[2].AddToItem(new MountableItem()
            {
                Name = "대충짱센갑옷123",
                Description = "수련에 도움을 주는 갑옷입니다.",
                Price = 1000,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 4,
                Own = false,
                Equip = false
            }, 1);
            quests[2].AddToItem(new MountableItem()
            {
                Name = "대충짱센값옷346678",
                Description = "수련에 도움을 주는 갑옷입니다.",
                Price = 1000,
                Type = ITEMTYPE.ARMOR,

                Attack = 0,
                Defense = 4,
                Own = false,
                Equip = false
            }, 1);
        }
    }
}
