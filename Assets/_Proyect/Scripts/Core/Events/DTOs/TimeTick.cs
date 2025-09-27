namespace Project.Core.Events.DTOs
{
    public struct TimeTick
    {
        public int minutes;
        public int seconds;
        public TimeTick(int m, int s) { minutes = m; seconds = s; }
    }
}
