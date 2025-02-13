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

        // save Wrapper
        private List<SaveQuestWrapper> saveWrapper;

        // 퀘스트 수락 or 퀘 받기 or 보상받기 선택 시 실행할 메서드 저장해놓을 Action
        private Action? actionOne;
        private Action? actionTwo;

        public List<SaveQuestWrapper> SaveWrapper => saveWrapper;

        // 생성자
        private QuestManager()
        {
            // 컨테이너 초기화
            stringByQuest = new Dictionary<string, Quest>();
            performableQuests = new List<Quest>();
            doneQuest = new List<Quest>();
            saveWrapper = new List<SaveQuestWrapper>();

            rewardOptionByState = new string[2];


            // 퀘스트 리스트 
            InitQuestList();

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

            // 수행가능한 퀘스트 이름만 빼서 배열로 저장 (LINQ)
            questOption = performableQuests.Select(q => q.QuestName).ToArray();
            // 목록(리스트) 출력
            DisplayManager.PrintMenu(questOption);

            Console.WriteLine("\n0. 로비로 돌아가기 \n");

            // player input 
            int input = UtilManager.PlayerInput(0, questOption.Length);

            // 0 : 입력하면 되돌아가기 => 퀘스트화면으로
            if (input == 0)
                GameManager.Instance.ChangeScene(SceneState.LobbyManager);

            // 1 ~ N 퀘스트 고르기 성공
            // 1. 현재 퀘스트 
            currQuest = performableQuests[input - 1];
            // 2. 완료여부에 따라 state변화 (내부에서 처리함)
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

            Console.WriteLine("-" + currQuest.QuestProgress() + '\n');
            Console.WriteLine("-" + currQuest.RewardItemAndGold() + '\n');
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
            int input = UtilManager.PlayerInput(1, 2);

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
                // ##TODO : 획득할 아이템의 갯수도 dicrionary에 있긴한데
                // inventory에 갯수만큼 획득 이라는 기능이 없어서 보류

                // 아이템 획득처리 type이 item이라 Mountable로 형변환
                InventoryManager.instance.RewardInstnace.SetItem((MountableItem)temp.Key);
            }

            // 골드추가
            // 플레이어 골드 += 현재 퀘스트 클리어골드
            PlayerManager.Instance._Player.gold += currQuest.QuestGold;

            // 최종 완료 퀘스트 세팅 
            RemovePerfomListAndAddToChild();

            WaitAndReturnToQuest();
        }

        private void RemovePerfomListAndAddToChild()
        {
            // 현재 퀘스트의 state를 done으로
            // 퀘스트 수락 -> 현재 퀘스트를 accept 로 바꾸기
            currQuest.ChangeState(QuestState.done);

            // currQuest와 같은 quest 반환
            var temp = performableQuests.Find(quest => quest.Equals(currQuest));

            // 수행가능 리스트에서 삭제
            if (temp != null)
            {
                performableQuests.Remove(temp);
            }

            // done 리스트에 추가
            doneQuest.Add(currQuest);

            // 현재 퀘스트의 child리스트에 접근해서 가능한 퀘스트리스트에 넣어야 함 
            for (int i = 0; i < currQuest.ChildQuest.Count; i++)
            {
                performableQuests.Add(currQuest.ChildQuest[i]);
            }
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
            Quest kill1 = new MonsterKillQuest
                (name: "마을을 위협하는 미니언 처치"
                , tooltip: "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\r\n" +
                    "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\r\n" +
                    "모험가인 자네가 좀 처치해주게!\r\n",
                perForm: "",
                rewardGold: 1000);
            Quest kill2 = new MonsterKillQuest
               (name: "대포 미니언 처치"
               , tooltip: "미니언들을 처지하니 대포미니언이 나타났군!\r\n" +
                   "저정도는 잡을 수 있겟지!\r\n",
               perForm: "",
               rewardGold: 2000);
            Quest kill3 = new MonsterKillQuest
               (name: "공허충 처치하기"
               , tooltip: "저건 대체 뭐란말인가! \r\n" +
                   "마을에 공허충이 돌아다디는게 말이되나 모험가? \r\n" +
                   "싹다 잡고오게!\r\n",
               perForm: "",
               rewardGold: 3000);
            Quest kill4 = new MonsterKillQuest
               (name: "피니셔 - 세상의멸망 "
               , tooltip: "세상이 멸망하려나보군 \r\n" +
                   "미니언,대포미니언,공허충이 한번에 마을에 나타나다니 \r\n" +
                   "모험가, 마을을 지켜주게!\r\n",
               perForm: $"한개이상 장비를 낀 채로",
               rewardGold: 99999);

            // ? 역할 : null이 발생하면 메서드를 실행하지 않음 
            (kill1 as MonsterKillQuest)?.AddtoKillMonsterList("Minion", 5);
            (kill2 as MonsterKillQuest)?.AddtoKillMonsterList("Canon", 5);
            (kill3 as MonsterKillQuest)?.AddtoKillMonsterList("VoidMonster", 2);
            (kill4 as MonsterKillQuest)?.AddtoKillMonsterList("Minion", 40);
            (kill4 as MonsterKillQuest)?.AddtoKillMonsterList("Canon", 25);
            (kill4 as MonsterKillQuest)?.AddtoKillMonsterList("VoidMonster", 10);

            #endregion

            #region Equip 퀘스트 생성
            Quest equip1 = new EquiptQuest
                (name: "장비를 장착해보자"
                , tooltip: "모험가 자네 아직도 장비를 장착하지 않았는가?\r\n" +
                    "인벤토리에서 아무 장비나 장착하게! \r\n",
                perForm: "인벤토리에서 Armor 장비 장착하기",
                type: ITEMTYPE.ARMOR,
                wearCnt: 1,
                rewardGold: 500);
            Quest equip2 = new EquiptQuest
                (name: "더욱 더 강해지기!"
                , tooltip: "자네 맨손으로 싸우고있었나?\r\n" +
                    "무기를 끼면 강해진다네 \r\n" +
                    "인벤토리에서 무기를 착용하고 오게 \r\n",
                perForm: "인벤토리에서 Weapon 장비 장착하기",
                type: ITEMTYPE.WEAPON,
                wearCnt: 1,
                rewardGold: 1500);
            Quest equip3 = new EquiptQuest
                (name: "인벤토리가 가득!"
                , tooltip: "무기와 장비를 장착했군 ! ?\r\n" +
                    "그럼 이제 인벤토리를 가득 채워보세 \r\n" +
                    "인벤토리에 15개 이상 아이템을 가지고있게 \r\n",
                perForm: "인벤토리에서 15개 이상 아이템 소유",
                type: null,
                wearCnt: 15,
                rewardGold: 99999);
            #endregion

            // 전체 딕셔너리에 넣기
            try
            {
                stringByQuest.Add(kill1.QuestName, kill1);
                stringByQuest.Add(kill2.QuestName, kill2);
                stringByQuest.Add(kill3.QuestName, kill3);
                stringByQuest.Add(kill4.QuestName, kill4);

                stringByQuest.Add(equip1.QuestName, equip1);
                stringByQuest.Add(equip2.QuestName, equip2);
                stringByQuest.Add(equip3.QuestName, equip3);
            }
            catch (Exception ex) { Console.WriteLine(ex); }

            try
            {
                saveWrapper.Add(new SaveQuestWrapper(kill1.QuestName, kill1.QuestState));
                saveWrapper.Add(new SaveQuestWrapper(kill2.QuestName, kill2.QuestState));
                saveWrapper.Add(new SaveQuestWrapper(kill3.QuestName, kill3.QuestState));
                saveWrapper.Add(new SaveQuestWrapper(kill4.QuestName, kill4.QuestState));

                saveWrapper.Add(new SaveQuestWrapper(equip1.QuestName, equip1.QuestState));
                saveWrapper.Add(new SaveQuestWrapper(equip2.QuestName, equip2.QuestState));
                saveWrapper.Add(new SaveQuestWrapper(equip3.QuestName, equip3.QuestState));
            }
            catch (Exception ex) { Console.WriteLine(ex); }


            // 트리 연결하기 
            // 연계퀘스트 내역은 는 피그마 확인해주세요!
            try
            {
                stringByQuest[kill1.QuestName].AddChild(kill2);
                stringByQuest[kill1.QuestName].AddChild(kill3);
                stringByQuest[kill2.QuestName].AddChild(kill4);

                stringByQuest[equip1.QuestName].AddChild(equip2);
                stringByQuest[equip2.QuestName].AddChild(equip3);
            }
            catch (Exception ex) { Console.WriteLine(ex); }

            // 보상 세팅
            try
            {
                stringByQuest[kill1.QuestName].AddToItem(InventoryManager.instance.RewardInstnace.GetItem("흙 검"), 1);
                stringByQuest[kill2.QuestName].AddToItem(InventoryManager.instance.RewardInstnace.GetItem("구리 검"), 1);
                stringByQuest[kill3.QuestName].AddToItem(InventoryManager.instance.RewardInstnace.GetItem("은 검"), 1);
                stringByQuest[kill4.QuestName].AddToItem(InventoryManager.instance.RewardInstnace.GetItem("철 활"), 1);
                stringByQuest[kill4.QuestName].AddToItem(InventoryManager.instance.RewardInstnace.GetItem("칠흑나무 활"), 1);

                stringByQuest[equip1.QuestName].AddToItem(InventoryManager.instance.RewardInstnace.GetItem("유니클로 셔츠"), 1);
                stringByQuest[equip2.QuestName].AddToItem(InventoryManager.instance.RewardInstnace.GetItem("워셔블밀라노 스웨터"), 1);
                stringByQuest[equip3.QuestName].AddToItem(InventoryManager.instance.RewardInstnace.GetItem("코튼엑스트라윔"), 1);
                stringByQuest[equip3.QuestName].AddToItem(InventoryManager.instance.RewardInstnace.GetItem("플러피얀후리스풀집 재킷"), 1);
            }
            catch (Exception ex) { Console.WriteLine(ex); }


            // 수행가능한 퀘스트 추가 
            try
            {
                performableQuests.Add(kill1);
                performableQuests.Add(equip1);
            }
            catch (Exception ex) { Console.WriteLine(ex); }

        }
    }
}
