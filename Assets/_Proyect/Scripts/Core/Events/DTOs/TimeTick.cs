namespace Project.Core.Events.DTOs
{
    public struct TimeTick
    {
        public readonly int minutes;
        public readonly int seconds;
        public TimeTick(int m, int s) { minutes = m; seconds = s; }
    }
}
