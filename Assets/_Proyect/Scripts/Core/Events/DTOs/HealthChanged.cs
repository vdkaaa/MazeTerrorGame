namespace Project.Core.Events.DTOs
{
    public struct HealthChanged
    {
        public readonly float current;
        public readonly float max;
        public HealthChanged(float current, float max) { this.current = current; this.max = max; }
        public readonly float Normalized() => max <= 0 ? 0f : current / max;
    }
}
