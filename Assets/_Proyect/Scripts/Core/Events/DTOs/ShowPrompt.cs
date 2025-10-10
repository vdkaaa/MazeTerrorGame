namespace Project.Core.Events.DTOs
{
    public struct ShowPrompt
    {
        public readonly string message;
        public readonly float duration; // segs; si <=0, mostrar indefinido
        public ShowPrompt(string message, float duration = 0f) { this.message = message; this.duration = duration; }
    }
}
