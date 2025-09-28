using UnityEngine;
using Project.Core.Events.DTOs;

namespace Project.Gameplay.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth = 100f;
        [SerializeField] private MonoBehaviour eventBusSource;
        private IEventBus _bus;

        private void Awake()
        {
            _bus = eventBusSource as IEventBus;
            Publish();
        }

        public bool IsDead => currentHealth <= 0f;

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            currentHealth = Mathf.Max(0f, currentHealth - Mathf.Abs(amount));
            Publish();
        }

        public void Heal(float amount)
        {
            if (IsDead) return;
            currentHealth = Mathf.Min(maxHealth, currentHealth + Mathf.Abs(amount));
            Publish();
        }

        private void Publish() => _bus?.Publish(new HealthChanged(currentHealth, maxHealth));
    }
}
