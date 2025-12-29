using UnityEngine;
using System.Collections.Generic;
using Work.Utils.EventBus;
using Work.Characters.Events;

namespace Work.Interact.Code
{
    [RequireComponent(typeof(Collider))]
    public class Interacter : MonoBehaviour
    {
        [SerializeField] private LayerMask interactLayer;

        public bool Interactable { get; private set; } = true;

        // Collider 대신 인터페이스를 직접 저장하여 캐싱
        private readonly List<IInteractable> _interactables = new();
        private IInteractable _currentInteractable;

        private void OnEnable() => Bus<CharacterInteractionEvent>.Events += OnInteractEvent;
        private void OnDisable() => Bus<CharacterInteractionEvent>.Events -= OnInteractEvent;

        private void Update()
        {
            // 이동 중에도 거리가 변하므로 지속적으로 갱신
            if (_interactables.Count > 0)
                UpdateClosestInteractable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!Interactable || !IsInteractableLayer(other.gameObject.layer)) return;

            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                _interactables.Add(interactable);
                UpdateClosestInteractable();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                _interactables.Remove(interactable);
                UpdateClosestInteractable();
            }
        }

        private void UpdateClosestInteractable()
        {
            IInteractable nearest = null;
            float minDistanceSqr = float.MaxValue;
            Vector3 currentPos = transform.position;

            // 리스트를 순회하며 가장 가까운 객체 탐색 (역순회로 안전한 삭제 지원)
            for (int i = _interactables.Count - 1; i >= 0; i--)
            {
                var item = _interactables[i];
                
                // 객체가 파괴되었을 경우 리스트에서 제거
                if (item == null || item.Equals(null))
                {
                    _interactables.RemoveAt(i);
                    continue;
                }

                // IInteractable은 Component라고 가정하고 위치 접근
                float distSqr = Vector3.SqrMagnitude(currentPos - ((Component)item).transform.position);
                if (distSqr < minDistanceSqr)
                {
                    minDistanceSqr = distSqr;
                    nearest = item;
                }
            }

            // 대상이 변경되었을 때만 상태 갱신
            if (_currentInteractable != nearest)
            {
                _currentInteractable?.SetInteractable(false);
                _currentInteractable = nearest;
                _currentInteractable?.SetInteractable(true);
            }
        }

        private bool IsInteractableLayer(int layer) => (interactLayer & (1 << layer)) != 0;

        public void OnInteractEvent(CharacterInteractionEvent evt) { _currentInteractable?.Interact();}
    }
}