using Blade.Core;

namespace Blade.Events
{
    public static class CameraEvents
    {
        public static ImpulseEvent ImpulseEvent = new ImpulseEvent();
    }

    public class ImpulseEvent : GameEvent
    {
        public float power;

        public ImpulseEvent Initializer(float power)
        {
            this.power = power;
            return this;
        }
    }
}