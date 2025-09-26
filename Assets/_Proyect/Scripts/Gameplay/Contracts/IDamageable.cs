namespace Project.Gameplay.Player
{
    public interface IDamageable
    {
        void TakeDamage(float amount);
        bool IsDead { get; }
    }
}
