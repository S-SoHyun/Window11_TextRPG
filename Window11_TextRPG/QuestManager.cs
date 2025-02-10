using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    internal class QuestManager : IScene
    {
        private List<Quest> quests;     // 퀘스트 컨테이너 
        private Quest currQuest;       // 현재 퀘스트 저장 
        private string[] questList;     // 퀘스트 씬 옵션 목록
        private string[] getReward;     // 보상받 or 돌아가기

        // 생성자
        private QuestManager()
        {
            // 퀘스트 리스트 
            InitQuestList();

            // quest에서 이름만 빼서 배열로 저장 (LINQ)
            questList = quests.Select(q => q.QuestName).ToArray();

            getReward = new string[2]
            {
                "보상 받기",
                "돌아가기"
            };
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
            DisplayManager.PrintMenu(questList);

            // player input 
            int input = UtilManager.PlayerInput(1, questList.Length);

            // 현재 퀘스트 
            currQuest = quests[input - 1];
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

            // 성공여부에 따라 print 다름 
            PrintReward();
        }

        private void PrintReward() 
        {
            // 성공하면 ?
            if (currQuest.IsCompleted()) 
            {
                // 목록(리스트) 출력
                DisplayManager.PrintMenu(getReward);

                // ##TODO : 보상 받기 생각해야함 !!!! 
            }

            // 아직 덜 성공 했으면 ?
            else 
            {
                Console.WriteLine("아직 퀘스트를 완료 하지 않으셨군요! ");
            }

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
                monstername: "",
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

            // 보상 아이템 세팅 
            // ##TODO : InventoryManager에서 들고와야함 


        }
    }
}
