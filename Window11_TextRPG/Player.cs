using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    public class Player : IMove
    {
        public string name { get; set; }
        public int level { get; set; }
        public string job { get; set; } 
        public float atk { get; set; } 
        public float def { get; set; } 

        public int maxhp { get; set; } 
        public int hp { get; set; } 

        public int maxmp { get; set; }
        public int mp { get; set; }

        public int gold { get; set; }

        public int exp { get; set; }
        public Item weapon { get; set; } = null;
        public Item armor { get; set; } = null;
        [JsonIgnore]
        public List<Skill> skills { get; set; }
        public int stage { get; set; }

        public Player(string job, string name, int maxhp, float atk)
        {  //이름 ,직업, 체력, 최대 체력 , 공격력 ,방어력 ,돈, 경험치?
            this.name = name;
            this.job = job;
            this.maxhp = maxhp;
            this.atk = atk;
            maxmp = 50;

            level = 1;
            def = 5;
            hp = maxhp;
            mp = maxmp;
            gold = 1500;
            exp = 0;
            stage = 1;

            skills = new List<Skill>();
            int skillAtk = UtilManager.GetCeiling(atk);

            SetSkills(job);
        }
        public Player() { }

        public int Attack()
        {
            int playerAtk = UtilManager.GetCeiling((double) atk); ;
            int plusMinus = UtilManager.GetCeiling(playerAtk * 0.1);
            int attackDamage = UtilManager.getRandomInt(playerAtk - plusMinus, playerAtk + plusMinus);
            
            return attackDamage;
        }

        public (int finalDamage, int hit) AttackCalculator(int attackDamage)
        {
            int hit = 0;
            int finalDamage = 0;
            int atkhit = UtilManager.getRandomInt(0, 101);

            if (atkhit < 10)
            {
                hit = 1;  // 빗나감
                finalDamage = 0;
            }
            else if (atkhit < 25)
            {
                hit = 2;  // 치명타
                finalDamage = UtilManager.GetCeiling(attackDamage * 1.6);
            }
            else
            {
                hit = 3;  // 기본 적중
                finalDamage = attackDamage;
            }

            return (finalDamage, hit); // 두 개의 값을 튜플로 반환
        }

        public int GetExpForNextLevel()
        {
            return 10 + 5 * (level * (level - 1) / 2);
        }

        public void GainExp(int amount)
        {
            exp += amount;
            LevelUpCheck();
        }

        public void LevelUpCheck()
        {
            while (exp >= GetExpForNextLevel()) 
            {
                exp -= GetExpForNextLevel();
                level++;
                atk += 0.5f;
                def += 1;
            }
        }

        public bool Hpcheck()
        {
            if (hp <= 0)
            {
                return true;
            }

            return false;
        }

        public void SetSkills(string job)
        {
            switch (job)
            {
                case "전사":
                    skills.Add(new Skill("알파 스트라이크", "공격력 * 3 로 하나의 적을 공격합니다.", 10, (int)DungeonManager.SkillType.one, SkillManager.AlphaStrike));
                    skills.Add(new Skill("더블 스트라이크", "공격력 * 1.5 로 2명의 적을 랜덤으로 공격합니다.", 15, (int)DungeonManager.SkillType.random2, SkillManager.DoubleStrike));
                    break;
                case "마법사":
                    skills.Add(new Skill("파이어볼", "공격력 * 2 로 하나의 적을 공격합니다.", 15, (int)DungeonManager.SkillType.one, SkillManager.FireBall));
                    skills.Add(new Skill("메테오", "공격력 * 3 로 모든 적을 공격합니다.", 30, (int)DungeonManager.SkillType.all, SkillManager.Meteor));
                    break;
                case "도적":
                    skills.Add(new Skill("라이프 스틸", "공격력만큼 하나의 적의 체력을 훔칩니다.", 15, (int)DungeonManager.SkillType.one, SkillManager.LifeSteal));
                    skills.Add(new Skill("골드 어택", "공격력 + (현재골드 / 100)만큼 하나의 적을 공격합니다.", 15, (int)DungeonManager.SkillType.one, SkillManager.GoldAttack));
                    break;
                case "궁수":
                    skills.Add(new Skill("파워샷", "공격력 * 2 로 하나의 적을 공격합니다.", 15, (int)DungeonManager.SkillType.one, SkillManager.PowerShot));
                    skills.Add(new Skill("멀티샷", "(공격력 * 1.5) - 대상 수만큼 모든 적을 공격합니다.", 20, (int)DungeonManager.SkillType.all, SkillManager.MultiShot));
                    break;
            }
        }
    }
}


