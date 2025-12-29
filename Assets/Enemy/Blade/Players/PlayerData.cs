using System;
using Blade.Combat;
using Blade.Core;
using Blade.Entities;
using Blade.Events;
using Blade.SkillSystem;
using Blade.StatSystem;
using UnityEngine;

namespace Blade.Players
{
    public class PlayerData : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameEventChannelSO playerChannel;
        private Entity _entity;
        private EntityStatCompo _statCompo;
        private SkillComponent _skillCompo;
        
        [field: SerializeField] public int Gold { get; private set; }
        [field: SerializeField] public int Exp { get; private set; } = 0;
        [field: SerializeField] public int LevelUpExp { get; private set; } = 30;
        [field: SerializeField] public int Level { get; private set; } = 1;
        [field: SerializeField] public LevelUpTableSO LevelUpTable { get; private set; }

        [field: SerializeField] public int StatPoint { get; private set; } = 0;
        [field: SerializeField] public int SkillPoint { get; private set; } = 0;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStatCompo>();
            _skillCompo = entity.GetCompo<SkillComponent>();
            playerChannel.AddListener<AddExpEvent>(HandleAddExp);
            playerChannel.AddListener<RequestStat>(HandleRequestStat);
            playerChannel.AddListener<ChangeStatEvent>(HandleChangeStat);
            playerChannel.AddListener<SkillUpgradeEvent>(HandleSkillUpgrade);
            UpdateLevelUpExp();
        }

        private void Start()
        {
            playerChannel.RaiseEvent(PlayerEvents.PlayerExpEvent.Initializer(Exp, LevelUpExp));
            playerChannel.RaiseEvent(PlayerEvents.GoldChangeEvent.Initializer(Gold));
            _skillCompo.UpdateSkillTree(SkillPoint);
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<AddExpEvent>(HandleAddExp);
            playerChannel.RemoveListener<RequestStat>(HandleRequestStat);
            playerChannel.RemoveListener<ChangeStatEvent>(HandleChangeStat);
            playerChannel.RemoveListener<SkillUpgradeEvent>(HandleSkillUpgrade);
        }

        private void HandleSkillUpgrade(SkillUpgradeEvent evt)
        {
            if (SkillPoint > 0 && evt.targetSkill.CanUpgradeSkill(evt.upgradeData))
            {
                SkillPoint--;
                evt.targetSkill.UpgradeSkill(evt.upgradeData);
                _skillCompo.UpdateSkillTree(SkillPoint);
            }
            else
            {
                //나중에 팝업으로 띄워줘야 해.
                Debug.LogWarning("Skill Point is not enough");
            }
        }

        private void UpdateLevelUpExp()
        {
            LevelUpExp = LevelUpTable.GetRequireExpForNextLevel(Level + 1); //다음레벨 경험치.
        }

        private void HandleChangeStat(ChangeStatEvent evt)
        {
            StatSO target = _statCompo.GetStat(evt.targetStat);
            target.BaseValue += target.incrementStep * evt.stepAmount;
            StatPoint -= evt.stepAmount; //포인트 감소
            UpdateUI();
        }

        private void HandleRequestStat(RequestStat evt) => UpdateUI();
        

        private void UpdateUI()
        {
            var responseEvt = PlayerEvents.ResponseStat.Initializer(_statCompo, StatPoint);
            playerChannel.RaiseEvent(responseEvt);
        }


        private void HandleAddExp(AddExpEvent evt) => AddExp(evt.amount);

        
        public bool SpendGold(int amount)
        {
            if (Gold < amount) return false;
            Gold -= amount;
            playerChannel.RaiseEvent(PlayerEvents.GoldChangeEvent.Initializer(Gold));
            return true;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
            playerChannel.RaiseEvent(PlayerEvents.GoldChangeEvent.Initializer(Gold));
        }

        public void AddExp(int amount)
        {
            Exp += amount;
            if (Exp >= LevelUpExp)
            {
                LevelUpProcess();
            }
            playerChannel.RaiseEvent(PlayerEvents.PlayerExpEvent.Initializer(Exp, LevelUpExp));
        }

        private void LevelUpProcess()
        {
            Exp -= LevelUpExp;
            Level++;
            UpdateLevelUpExp();
            StatPoint += LevelUpTable.statPerLevel;
            SkillPoint += LevelUpTable.skillPerLevel;
            playerChannel.RaiseEvent(PlayerEvents.LevelUpEvent.Initializer(Level));
            _skillCompo.UpdateSkillTree(SkillPoint);
        }
    }
}