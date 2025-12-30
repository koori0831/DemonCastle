using System;
using System.Collections.Generic;
using System.Linq;
using Blade.Core;
using Blade.Entities;
using Blade.Events;
using UnityEngine;
using VHierarchy.Libs;

namespace Blade.SkillSystem
{
    public class SkillComponent : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private LayerMask whatIsTarget;
        [SerializeField] private int maxCheckCount;
        
        public Collider[] Colliders { get; private set; }

        private Entity _entity;
        private Dictionary<Type, Skill> _skillDict;
        
        [field: SerializeField] public Skill CurrentSkill { get; set; }
        [field: SerializeField] public GameEventChannelSO CameraChannel { get; private set; }
        [field: SerializeField] public GameEventChannelSO SoundChannel {get; private set;}
        [field: SerializeField] public GameEventChannelSO PlayerChannel {get; private set;}

        public void UpdateSkillTree(int skillPoint)
        {
            PlayerChannel.RaiseEvent(PlayerEvents.SkillTreeUpdateEvent.Initializer(_skillDict, skillPoint));
        }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            Colliders = new Collider[maxCheckCount];

            _skillDict = new Dictionary<Type, Skill>();
            GetComponentsInChildren<Skill>().ToList()
                .ForEach(skill => _skillDict.Add(skill.GetType(), skill));
            
        }

        public T GetSkill<T>() where T : Skill
        {
            Type skillType = typeof(T);
            Skill skill = _skillDict.GetValueOrDefault(skillType);
            Debug.Assert(skill != null, $"Finding skill type is not exist : {skillType}");

            return skill as T;
        }

        public void AddSkill(Skill skill)
            => _skillDict.Add(skill.GetType(), skill);

        public void RemoveSkill(Skill skill)
            => _skillDict.Remove(skill.GetType());

        public int GetEnemiesInRange(Vector3 position, float range)
            => Physics.OverlapSphereNonAlloc(position, range, Colliders, whatIsTarget);

        public Entity FindClosestEnemy(Vector3 position, float range)
        {
            Entity findEnemy = null;
            int cnt = GetEnemiesInRange(position, range);

            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < cnt; i++)
            {
                if (Colliders[i].TryGetComponent(out Entity enemy) == false
                    || enemy.IsDead)
                    continue;

                float distance = Vector3.Distance(position, enemy.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    findEnemy = enemy;
                }
            }

            return findEnemy;
        }
    }
}