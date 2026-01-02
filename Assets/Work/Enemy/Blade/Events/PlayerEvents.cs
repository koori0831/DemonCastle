using System;
using System.Collections.Generic;
using Blade.Core;
using Blade.SkillSystem;
using Blade.SkillSystem.Upgrade;
using Blade.StatSystem;
using Unity.XR.OpenVR;

namespace Blade.Events
{
    public static class PlayerEvents
    {
        public static readonly PlayerDeadEvent PlayerDead = new PlayerDeadEvent();
        public static readonly PlayerHealthEvent PlayerHealthEvent = new PlayerHealthEvent();
        public static readonly PlayerExpEvent PlayerExpEvent = new PlayerExpEvent();
        public static readonly AddExpEvent AddExpEvent = new AddExpEvent();
        public static readonly GoldChangeEvent GoldChangeEvent = new GoldChangeEvent();
        public static readonly LevelUpEvent LevelUpEvent = new LevelUpEvent();
        public static readonly RequestStat RequestStat = new RequestStat();
        public static readonly ResponseStat ResponseStat = new ResponseStat();
        public static readonly ChangeStatEvent ChangeStatEvent = new ChangeStatEvent();
        public static readonly SkillTreeUpdateEvent SkillTreeUpdateEvent = new SkillTreeUpdateEvent();
        public static readonly SkillUpgradeEvent SkillUpgradeEvent = new SkillUpgradeEvent();
    }

    public class SkillUpgradeEvent : GameEvent
    {
        public Skill targetSkill;
        public SkillUpgradeSO upgradeData;

        public SkillUpgradeEvent Initializer(Skill skill, SkillUpgradeSO data)
        {
            targetSkill = skill;
            upgradeData = data;
            return this;
        }
    }
    
    public class SkillTreeUpdateEvent : GameEvent
    {
        public Dictionary<Type, Skill> skills;
        public int skillPoint;

        public SkillTreeUpdateEvent Initializer(Dictionary<Type, Skill> skills, int skillPoint)
        {
            this.skills = skills;
            this.skillPoint = skillPoint;
            return this;
        }
    }
    
    public class ChangeStatEvent : GameEvent
    {
        public StatSO targetStat;
        public int stepAmount;

        public ChangeStatEvent Initializer(StatSO target, int amount = 1)
        {
            targetStat = target;
            stepAmount = amount;
            return this;
        }
    }
    
    public class RequestStat : GameEvent
    { }

    public class ResponseStat : GameEvent
    {
        public EntityStatCompo statCompo;
        public int statPoint;

        public ResponseStat Initializer(EntityStatCompo statCompo, int statPoint)
        {
            this.statCompo = statCompo;
            this.statPoint = statPoint;
            return this;
        }
    }

    public class LevelUpEvent : GameEvent
    {
        public int currentLevel;

        public LevelUpEvent Initializer(int level)
        {
            currentLevel = level;
            return this;
        }
    }
    
    public class GoldChangeEvent : GameEvent
    {
        public int goldAmount;

        public GoldChangeEvent Initializer(int amount)
        {
            this.goldAmount = amount;
            return this;
        }
    }
    
    public class PlayerDeadEvent : GameEvent
    {
    }

    public class PlayerHealthEvent : GameEvent
    {
        public float health;
        public float maxHealth;

        public PlayerHealthEvent Initializer(float health, float maxHealth)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            return this;
        }
    }

    public class PlayerExpEvent : GameEvent
    {
        public float currentExp;
        public float maxExp;

        public PlayerExpEvent Initializer(float current, float max)
        {
            this.currentExp = current;
            this.maxExp = max;
            return this;
        }
    }

    public class AddExpEvent : GameEvent
    {
        public int amount;

        public AddExpEvent Initializer(int amount)
        {
            this.amount = amount;
            return this;
        }
    }
    
   
}