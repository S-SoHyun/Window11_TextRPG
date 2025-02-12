using System.Linq.Expressions;
using System.Net;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    public class SaveQuestWrapper
    {
        private string name;
        private QuestState questType;

        public SaveQuestWrapper(string n , QuestState state ) 
        {
            this.name = n;
            this.questType = state;
        }

        public string Name => name;
        public QuestState QuestType => questType;
    }

    public abstract class Quest
    {
        // 필드 
        protected string questName;   // 퀘스트이름
        protected string questStory;  // 퀘스트 스토리 (ex) 미니언들이 너무 많아졋다고 생각~
        protected int rewardGold;               // 리워드 
        protected string questPerform;          // 수행내역 
        protected QuestState questState;        // 수행 스탯 

        // 컨테이너
        protected Dictionary<Item, int> rewardItemByCount;        // 보상 아이템별 count
        protected Func<bool> completeQuest;                       // 성공여부 Func 
        // 단일 반환값을 사용해서 delegate보다는 Func이 더 알맞다고생각
        // 델리게이트는 메서드를 한개의 이벤트 처럼 사용할 때 유용하다고 생각 

        // 트리구조
        protected Quest parentQuest;
        protected List<Quest> childQuest;

        // 프로퍼티
        public string QuestName => questName;
        public string QuestStory => questStory;
        public int QuestGold => rewardGold;
        public string QuestPerform => questPerform;
        public QuestState QuestState { get => questState; set { questState = value; } }
        public Dictionary<Item, int> RewardItemByCount => rewardItemByCount;
        public Quest ParentQuest { get => parentQuest; set { parentQuest = value; } }
        public List<Quest> ChildQuest => childQuest;

        public Quest(string name, string tooltip, string questPerfom , int rewardGold)
        {
            this.questName = name;
            this.questStory = tooltip;
            this.questPerform = questPerfom;
            this.questState = QuestState.beforeReceive;     // 생성할 때, 받기전으로 설정 
            this.rewardGold = rewardGold;

            if (rewardItemByCount == null)
                rewardItemByCount = new Dictionary<Item, int>();
            if(childQuest == null)
                childQuest = new List<Quest>(); 
        }

        public void AddToItem(Item item, int count)
        {
            try
            {
                rewardItemByCount.Add(item, count);
            }
            catch
            {
                Console.WriteLine("Quest 클래스 : Item Dicrionary 추가 오류 ");
            }
        }

        public void ChangeState(QuestState state) 
        {
            this.questState = state;
        }

        public void CheckState() 
        {
            // 받기전이면 return 
            if (questState == QuestState.beforeReceive)
                return;

            // 조건을 만족하면
            if (completeQuest.Invoke()) 
            {
                // 완료 state로 변환 
                this.questState = QuestState.complete;
            }
        }

        // 하위에서 작성해야할 퀘스트 성공 조건
        public abstract bool TodoMission();

        // 퀘스트 진행내역
        public abstract string QuestProgress();

        // 보상 아이템 + 골드 string값 return
        public string RewardItemAndGold() 
        {
            string em = string.Empty;

            foreach (var temp in rewardItemByCount)
            {
                em += $"{temp.Key.Name} X {temp.Value} \n";
                em += $"{rewardGold} G \n";
            }

            // ex) 쓸만한 방패 x 1
            // ex) 500 G
            return em;
        }

        // child 자식 대입
        public void AddChild(Quest child) 
        {
            child.parentQuest = this;
            childQuest.Add(child);
        }
    }

    public class MonsterKillQuest : Quest
    {
        // 처치해야할 몬스터 
        // Monster 타입이면 더 좋을듯
        private List<string> monsterNameList;
        private List<int> killCountList;
        
        // 프로퍼티
        public List<string> MonsterNameList => monsterNameList;
        public List<int> KillCountList => killCountList;  

        public MonsterKillQuest(string name, string tooltip,string perForm , int rewardGold) : 
            base(name, tooltip , perForm , rewardGold)
        {
            // Func에 연결
            completeQuest += TodoMission;
        }

        public void AddtoKillMonsterList(string name , int cnt ) 
        {
            if (monsterNameList == null)
                monsterNameList = new List<string>();
            if (killCountList == null)
                killCountList = new List<int>();

            // 리스트에 각각 추가
            monsterNameList.Add(name);
            killCountList.Add(cnt);
        }

        public override bool TodoMission()
        {
            bool flag = true;

            // 지금까지 처치한 몬스터 수 
            for (int i = 0; i < monsterNameList.Count; i++) 
            {
                string nowMonster = monsterNameList[i];
                int countToKill = killCountList[i];

                // 잡은횟수
                int killCnt = DungeonManager.Instance.monsterCatches[nowMonster];

                // 처치한 몬스터가 kill count보다 낮으면 -> 실패
                if (killCnt < countToKill)
                { 
                    flag = false;
                    break;
                }
            }

            // for문안의 조건문에 걸리지않으면 -> killcount대로 다 잡은것 
            return flag;
        }

        public override string QuestProgress()
        {
            string str = String.Empty;

            // 지금까지 처치한 몬스터 수 
            for (int i = 0; i < monsterNameList.Count; i++)
            {
                string nowMonster = monsterNameList[i];
                int nowKillCnt = killCountList[i];

                // 잡은횟수
                int killCnt = DungeonManager.Instance.monsterCatches[nowMonster];

                str += $"{this.questPerform} {nowMonster} 몬스터 처지하기 ( {killCnt} / {nowKillCnt} ) \n";
            }

            // ex) 미니언 killcount만큼 처지 2 / 5
            // ex) 대포 미니언 killcount만큼 처지 1 / 5
            return str;
        }

    }

    public class EquiptQuest : Quest
    {
        private ITEMTYPE? questItemType;
        private int wearCount;

        public EquiptQuest(string name, string tooltip, string perForm , ITEMTYPE? type , int wearCnt , int rewardGold) 
            : base(name, tooltip,  perForm , rewardGold)
        {
            this.questItemType = type;
            this.wearCount = wearCnt;

            // Func에 연결
            completeQuest += TodoMission;
        }

        public override bool TodoMission()
        {
            // type별 조건에 만족하는 갯수가 착용해야하는 갯수보다 많으면 (=>성공)
            if ( typeByStisFiedCount() >= wearCount)
                return true;

            return false;
        }

        public override string QuestProgress()
        {
           
            // ex) 장비 착용하기 ( 조건에 만족하는 아이템 갯수  / 1 )
            return $"{questPerform} ( {typeByStisFiedCount()} / {wearCount} )";
        }

        private int typeByStisFiedCount() 
        {
            int count = 0;
            //  type이 null 이면 ?
            // 인벤토리 리스트의 count를 검사해야함
            if (questItemType == null)
            {
                // own 변수가 true인 것만 count
                count = InventoryManager.instance.mountableItems
                        .Count(item => item.Own);
            }

            // 조건 : 아이템타입이 questType 와 같고, equip가 true이면 
            // 조건에 맞는 count 
            else
            {
                count = InventoryManager.instance.mountableItems
                    .Count(item => item.Type == questItemType && item.Equip);
            }

            return count;
        }
    }

}
