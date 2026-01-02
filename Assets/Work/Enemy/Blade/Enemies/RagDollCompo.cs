using System.Collections.Generic;
using System.Linq;
using Blade.Entities;
using UnityEngine;

namespace Blade.Enemies
{
    public class RagDollCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Transform ragDollParentTrm;
        [SerializeField] private LayerMask whatIsBody;

        private List<RagDollPart> _partList;
        private Collider[] _results;

        private RagDollPart _defaultPart;
        
        public void Initialize(Entity entity)
        {
            _results = new Collider[1];
            _partList = ragDollParentTrm.GetComponentsInChildren<RagDollPart>().ToList();

            foreach (RagDollPart part in _partList)
            {
                part.InitializePart();
            }
            
            Debug.Assert(_partList.Count > 0, $"No ragdoll part found in {ragDollParentTrm.name}");
            _defaultPart = _partList[0]; //기본 파트를 넣어준다.
            SetRagDollActive(false);
            SetColliderActive(false);
        }

        public void SetRagDollActive(bool isActive)
        {
            _partList.ForEach(part => part.SetRagDollActive(isActive));
        }
        
        public void SetColliderActive(bool isActive)
        {
            _partList.ForEach(part => part.SetCollider(isActive));
        }

        public void AddForceToRagDoll(Vector3 force, Vector3 position)
        {
            int count = Physics.OverlapSphereNonAlloc(position, 0.5f, _results, whatIsBody);
            if (count > 0)
            {
                _results[0].GetComponent<RagDollPart>().KnockBack(force, position);
            }
            else
            {
                _defaultPart.KnockBack(force, position);
            }
        }
    }
}