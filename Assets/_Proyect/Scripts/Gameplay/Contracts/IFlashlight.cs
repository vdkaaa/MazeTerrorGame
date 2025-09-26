namespace Project.Gameplay.Player
{
    public interface IFlashlight
    {
        void Toggle();
        void SetOn(bool on);
        float BatteryNormalized(); // 0..1
    }
}
