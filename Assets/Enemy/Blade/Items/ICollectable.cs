using Blade.Entities;

namespace Blade.Items
{
    public interface ICollectable
    {
        bool CanCollect { get; }
        void Collect(Entity entity);
    }
}