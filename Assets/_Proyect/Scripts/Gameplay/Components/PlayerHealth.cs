using UnityEngine;

namespace Project.Gameplay.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth = 100f;

        public bool IsDead => currentHealth <= 0f;

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            currentHealth = Mathf.Max(0f, currentHealth - Mathf.Abs(amount));
            // TODO: emitir evento PlayerDamaged/PlayerDied
        }
    }
}
