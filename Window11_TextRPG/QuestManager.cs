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
        beforeReceive,  // 받기전
        afterReceive,   // 받은 후 
        complete,       // 받은 후 보상수령 가능한 상태
        done            // 보상수령을 했다면 아얘 끝난 상태
    }


    internal class QuestManager : IScene
    {
        private string[] questOption;     // 퀘스트 씬 옵션 목록
        private string[] rewardOptionByState;     // 스탯 별 출력해야할 옵션 : 수락 or 퀘받아가세요 or 보상받기

        // 퀘스트 컨테이너
        private Dictionary<string, Quest> stringByQuest;    // 전체 퀘스트 컨테이너 
        private Quest currQuest;                            // 현재 퀘스트 저장 

        private List<Quest> performableQuests;  // 수행가능 퀘스트
        private List<Quest> doneQuest;          // 아얘 끝난 퀘스트 

        // 퀘스트 수락 or 퀘 받기 or 보상받기 선택 시 실행할 메서드 저장해놓을 Action
        private Action? actionOne;
        private Action? actionTwo;

        // 생성자
        private QuestManager()
        {
            // 컨테이너 초기화
            stringByQuest       = new Dictionary<string, Quest>();
            performableQuests   = new List<Quest>();
            doneQuest           = new List<Quest>();

            rewardOptionByState = new string[2];


            // 퀘스트 리스트 
            InitQuestList();

            // 수행가능한 퀘스트 이름만 빼서 배열로 저장 (LINQ)
            questOption = performableQuests.Select(q => q.QuestName).ToArray();

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
            Console.Clear();

            // 목록(리스트) 출력
            DisplayManager.PrintMenu(questOption);

            Console.WriteLine("\n0. 로비로 돌아가기 \n");

            // player input 
            int input = UtilManager.PlayerInput(0, questOption.Length);
            
            // 4 입력하면 되돌아가기 => 퀘스트화면으로
            if (input == 0)
                GameManager.Instance.ChangeScene(SceneState.LobbyManager);

            // 현재 퀘스트 
            currQuest = quests[input - 1];
            // 완료여부에 따라 state변화 (내부에서 처리함)
            currQuest.CheckState();

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
            actionOne = null;
            actionTwo = null;

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
                    actionOne += AcquirReward;
                    actionTwo += ReturnToMenu;
                    break;
            }

            // 목록(리스트) 출력
            DisplayManager.PrintMenu(rewardOptionByState);

            // 플레이어 입력
            int input = UtilManager.PlayerInput(1,2);

            // 1이면 action첫번째꺼, 2이면 action 두번째꺼
            if (input == 1)
                actionOne.Invoke();
            else 
                actionTwo.Invoke();
        }

        private void Acception() 
        {
            // 퀘스트 수락 -> 현재 퀘스트를 accept 로 바꾸기
            currQuest.ChangeState(QuestState.afterReceive);

            WaitAndReturnToQuest();
        }
        private void DeclineQuest() 
        {
            Console.WriteLine("퀘스트를 거절하셨습니다 ");

            WaitAndReturnToQuest();
        }
        private void CompleteAndComeBack() 
        {
            // 퀘스트 완료필요 -> 완료하고 오세요 ! 
            Console.WriteLine("퀘스트를 완료하고 오세요!");

            WaitAndReturnToQuest();
        }

        private void ReturnToMenu() 
        {
            WaitAndReturnToQuest();
        }

        private void AcquirReward() 
        {
            Console.WriteLine("보상을 받았습니다. 인벤토리를 확인해주세요!");

            // 보상 받기
            // currQuest의 보상 컨테이너에 접근해서 아이템 획득
            foreach (var temp in currQuest.RewardItemByCount)
            {
                // 아이템 획득처리 type이 item이라 Mountable로 형변환
                InventoryManager.instance.RewardInstnace.SetItem((MountableItem)temp.Key);

                // 골드추가
                // 플레이어의 골드 프로퍼티에 접근
                // ##TODO : PlayerManager에 player변수 프로퍼티 만들어달라고하기

            }

            WaitAndReturnToQuest();
        }

        // 3초후 퀘스트화면으로 돌아가기
        private void WaitAndReturnToQuest() 
        {
            Console.WriteLine("1초후 퀘스트 화면으로 돌아갑니다");

            UtilManager.DelayForSecond(1);

            // 퀘스트 메뉴로 돌아가기
            GameManager.Instance.ChangeScene(SceneState.QuestManager);
        }


        private void InitQuestList()
        {
            

            // 1번 : 미니언처치
            // -> Dungeon에서 Monster 생성 후 몇마리 처치 했는지 저장 필요함 
            // 2번 : armor 장착
            // 3번 : weapon 장착
            // -> InventoryManager안에 아이템list가 있는데 item에는 장착했는지 여부에 대한 enum 이 있어서
            // enum 기준으로 LINQ 한 다음에 배열이 0이상이면 있음 없음 정도로 해보면될듯

            #region Monster Kill 퀘스트 생성 
            Quest kill1 =new MonsterKillQuest
                (name: "마을을 위협하는 미니언 처치"
                , tooltip: "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\r\n" +
                    "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\r\n" +
                    "모험가인 자네가 좀 처치해주게!\r\n",
                perForm : "Minion 처치하기");
            Quest kill2 = new MonsterKillQuest
               (name: "마을을 위협하는 대포 미니언 처치"
               , tooltip: "미니언들을 처지하니 대포미니언이 나타났군!\r\n" +
                   "모험가인 자네가 좀 처치해주게!\r\n",
               perForm: "Canon 처치하기");
            Quest kill3 = new MonsterKillQuest
               (name: "마을을 위협하는 미니언 처치"
               , tooltip: "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\r\n" +
                   "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\r\n" +
                   "모험가인 자네가 좀 처치해주게!\r\n",
               perForm: "VoidMonster 처치하기");
            Quest kill4 = new MonsterKillQuest
               (name: "마을을 위협하는 미니언 처치"
               , tooltip: "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\r\n" +
                   "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\r\n" +
                   "모험가인 자네가 좀 처치해주게!\r\n",
               perForm: "몬스터 처치하기");

            // ? 역할 : null이 발생하면 메서드를 실행하지 않음 
            (kill1 as MonsterKillQuest)?.AddtoKillMonsterList("Minion", 5);
            (kill2 as MonsterKillQuest)?.AddtoKillMonsterList("Canon", 5);
            (kill3 as MonsterKillQuest)?.AddtoKillMonsterList("VoidMonster", 2);
            (kill4 as MonsterKillQuest)?.AddtoKillMonsterList("Minion", 40);
            (kill4 as MonsterKillQuest)?.AddtoKillMonsterList("Canon", 25);
            (kill3 as MonsterKillQuest)?.AddtoKillMonsterList("VoidMonster", 10);

            #endregion

            #region Equip 퀘스트 생성
            Quest equip1 = new EquiptQuest
                (name: "장비를 장착해보자"
                , tooltip: "모험가 자네 아직도 장비를 장착하지 않았는가?\r\n" +
                    "인벤토리에서 아무 장비나 장착하게! \r\n",
                perForm: "인벤토리에서 Armor 장비 장착하기",
                type: ITEMTYPE.ARMOR,
                wearCnt: 1);
            Quest equip2 = new EquiptQuest
                (name: "더욱 더 강해지기!"
                , tooltip: "자네 맨손으로 싸우고있었나?\r\n" +
                    "무기를 끼면 강해진다네 \r\n" +
                    "인벤토리에서 무기를 착용하고 오게 \r\n",
                perForm: "인벤토리에서 Weapon 장비 장착하기",
                type: ITEMTYPE.WEAPON,
                wearCnt: 1);
            Quest equip3 = new EquiptQuest
                (name: "인벤토리가 가득!"
                , tooltip: "무기와 장비를 장책했군 ! ?\r\n" +
                    "그럼 이제 인벤토리를 가득 채워보세 \r\n" +
                    "인벤토리에 15개 이상 아이템을 가지고있게 \r\n",
                perForm: "인벤토리에서 15개 이상 아이템 소유",
                type: null,
                wearCnt: 15);
            #endregion

            // 보상 아이템 세팅
            stringByQuest.Add(kill1.QuestName , kill1);
            stringByQuest.Add(kill2.QuestName , kill2);
            stringByQuest.Add(kill3.QuestName , kill3);
            stringByQuest.Add(kill4.QuestName , kill4);
            
            stringByQuest.Add(equip1.QuestName , equip1);
            stringByQuest.Add(equip2.QuestName , equip2);
            stringByQuest.Add(equip3.QuestName , equip3);

            // 트리 연결하기 
            // 연계퀘스트 내역은 는 피그마 확인해주세요!
            stringByQuest[kill1.QuestName].AddChild(kill2);
            stringByQuest[kill1.QuestName].AddChild(kill3);
            stringByQuest[kill2.QuestName].AddChild(kill4);

            stringByQuest[equip1.QuestName].AddChild(equip2);
            stringByQuest[equip2.QuestName].AddChild(equip3);

            // 수행가능한 퀘스트 추가 
            performableQuests.Add(kill1);
            performableQuests.Add(equip1);
        }
    }
}
