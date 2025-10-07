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
        public float Max => maxHealth;
        public float Current => currentHealth;

        public void SetMax(float v) { maxHealth = Mathf.Max(1f, v); }
        public void SetCurrent(float v)
        {
            currentHealth = Mathf.Clamp(v, 0f, maxHealth);
            Publish(); //Vigilar si esto causa algun problema
        }

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
