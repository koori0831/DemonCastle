namespace Blade.SkillSystem
{
    public interface IChargeable
    {
        bool IsCharging { get; }
        void StartCharge();
        void ReleaseCharge();
        void CancelCharge();
    }
}