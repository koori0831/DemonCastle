using UnityEngine;
using Work.Entities;

namespace Work.Interact.Code
{
    public interface IInteractable
    {
        public void Interact(Entity interactor);
    }
}