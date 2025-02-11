using System.Linq.Expressions;
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


        public Quest(string name, string tooltip, string questPerfom)
        {
            this.questName = name;
            this.questStory = tooltip;
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

        public void SetRewardGold(int gold) 
        {
            this.rewardGold = gold;
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

        public MonsterKillQuest(string name, string tooltip,string perForm ,string monstername , int killCount) : 
            base(name, tooltip , perForm)
        {
            this.monsterName = monstername;
            this.killCount = killCount;

            // Func에 연결
            completeQuest += TodoMission;
        }

        public override bool TodoMission()
        {
            // 지금까지 처치한 몬스터 수 
            int nowKillMonsterCnt = DungeonManager.Instance.monsterCatches[MonsterName];

            // 해당하는 처치 몬스터가 Count가 넘으면 => return true
            if (nowKillMonsterCnt > killCount)
                return true;
            return false;
        }

        public override string QuestProgress()
        {
            // 지금까지 처치한 몬스터 수 
            int nowKillMonsterCnt = DungeonManager.Instance.monsterCatches[MonsterName];

            // ex) 미니언 killcount만큼 처지 2 / 5
            return $"{this.questPerform} ( {nowKillMonsterCnt} / {this.killCount} ) ";
        }

    }

    public class EquiptQuest : Quest
    {
        private ITEMTYPE questItemType;
        private int wearCount;

        public EquiptQuest(string name, string tooltip, string perForm , ITEMTYPE type , int wearCnt) 
            : base(name, tooltip,  perForm)
        {
            this.questItemType = type;
            this.wearCount = wearCnt;

            // Func에 연결
            completeQuest += TodoMission;
        }

        public override bool TodoMission()
        {
            bool flag = false;

            // 인벤토리 manager의 리스트에 접근
            // 조건 : 아이템타입이 questType 와 같고, equip가 true이면 
            // 조건에 만족하면 return true ( LINQ의 any 사용 )
            flag = InventoryManager.instance.mountableItems
                        .Any(item => item.Type == questItemType && item.Equip);

            /*
            var temp = from item in InventoryManager.instance.mountableItems
                       where item.Type == questItemType && item.Equip == true
                       select item;
            */
            return flag;
        }

        public override string QuestProgress()
        {
            // 조건 : 아이템타입이 questType 와 같고, equip가 true이면 
            // 조건에 맞는 count 
            int count = InventoryManager.instance.mountableItems
                .Count(item => item.Type == questItemType && item.Equip);

            // ex) 장비 착용하기 0 / 1
            return $"{questPerform} ( {count} / {wearCount} )";
        }
    }

}
