using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
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

        // 프로퍼티
        public string QuestName => questName;
        public string QuestStory => questStory;
        public int QuestGold => rewardGold;
        public string QuestPerform => questPerform;
        public QuestState QuestState { get => questState; set { questState = value; } }
        public Dictionary<Item, int> RewardItemByCount => rewardItemByCount;


        public Quest(string name, string tooltip, int reward , string questPerfom) 
        {
            this.questName = name;
            this.questStory = tooltip;
            this.rewardGold = reward;
            this.questPerform = questPerfom;
            this.questState = QuestState.beforeReceive;     // 생성할 때, 받기전으로 설정 

            if (rewardItemByCount == null)
                rewardItemByCount = new Dictionary<Item, int>();
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

        // Func을 실행
        public bool IsCompleted() 
        {
            // 만약 Func가 null 이면 false 
            if (completeQuest == null)
                return false;

            return completeQuest.Invoke();
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
                em += $"{temp.Key} X {temp.Value} \n";
                // ex) 쓸만한 방패 x 1
                em += $"{rewardGold} G ";
            }
            return em;
        }
    }

    public class MonsterKillQuest : Quest
    {
        // 처치해야할 몬스터 
        // Monster 타입이면 더 좋을듯
        private string monsterName;
        private int killCount;
        
        // 프로퍼티
        public string MonsterName => monsterName;
        public int KillCount => killCount;  

        public MonsterKillQuest(string name, string tooltip, int reward ,string perForm ,string monstername , int killCount) : 
            base(name, tooltip, reward , perForm)
        {
            this.monsterName = monstername;
            this.killCount = killCount;

            // Func에 연결
            completeQuest += TodoMission;
        }

        public override bool TodoMission()
        {
            // 해당하는 처치 몬스터가 Count가 넘으면 
            if (10 > killCount)
                return true;
            return false;
        }

        public override string QuestProgress()
        {
            return $"{this.questPerform} ( {0} / {this.killCount} ";

            // ##TODO : 0 < 이 부분은 DungeonManager의 함수 실행으로 가져오기 
        }

    }

    public class EquiptQuest : Quest
    {
        private ITEMTYPE questItemType;
        private int wearCount;

        public EquiptQuest(string name, string tooltip, int reward , string perForm , ITEMTYPE type , int wearCnt) 
            : base(name, tooltip, reward, perForm)
        {
            this.questItemType = type;
            this.wearCount = wearCnt;

            // Func에 연결
            completeQuest += TodoMission;
        }

        public override bool TodoMission()
        {
            // 인벤토리 manager에 접근해서 
            // ITEMTYPE에 맞는 list의 갯수를 들고오던가
            // 모든 Item을 담는 list라면 ITEMTYPE 타입별로 , LINQ로 갯수 알아내야함 
            return false;
        }

        public override string QuestProgress()
        {
            // ##TODO : 0 < 이 부분은 InventoryManager의 type에 g해당하는 부분 가져와야할듯 

            return $"{questPerform} ( {0} / {wearCount} ";
        }
    }

}
