namespace Project.Core.Events.DTOs
{
    public struct HealthChanged
    {
        public float current;
        public float max;
        public HealthChanged(float current, float max) { this.current = current; this.max = max; }
        public float Normalized() => max <= 0 ? 0f : current / max;
    }
}
