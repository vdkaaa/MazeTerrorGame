namespace Project.Gameplay.Player
{
    public interface IMovable
    {
        void SetMoveInput(float x, float y); // eje X,Y (WASD)
        void SetLookInput(float dx, float dy); // delta mouse
    }
}
