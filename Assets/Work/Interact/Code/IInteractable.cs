using Work.Characters;

namespace Work.Interact.Code
{
    public interface IInteractable
    {
        public void Interact();
        public void SetInteractable(bool isInteractable);
    }
}