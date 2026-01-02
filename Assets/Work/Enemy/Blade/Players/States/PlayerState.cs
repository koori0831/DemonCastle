using Blade.Entities;
using Blade.FSM;

namespace Blade.Players.States
{
    public abstract class PlayerState : EntityState
    {
        protected Player _player;
        protected readonly float _inputThreshold = 0.1f;
        
        protected PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
        }
    }
}