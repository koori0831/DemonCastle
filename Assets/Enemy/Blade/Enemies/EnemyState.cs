using Unity.Behavior;

namespace Blade.Enemies
{
    [BlackboardEnum]
    public enum EnemyState
    {
        IDLE = 0, PATROL = 1, CHASE = 2, ATTACK = 3, HIT = 4, DEAD = 5, WANDERING = 6
    }
}