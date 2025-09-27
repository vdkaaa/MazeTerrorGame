namespace Project.Core.Events.DTOs
{
    public struct ShowPrompt
    {
        public string message;
        public float duration; // segs; si <=0, mostrar indefinido
        public ShowPrompt(string message, float duration = 0f) { this.message = message; this.duration = duration; }
    }
}
