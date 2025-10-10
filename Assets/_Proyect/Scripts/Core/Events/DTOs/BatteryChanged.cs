namespace Project.Core.Events.DTOs
{
    public struct BatteryChanged
    {
        public readonly float normalized; // 0..1
        public BatteryChanged(float n) { normalized = n; }
    }
}
