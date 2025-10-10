using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Data; 

namespace Project.Gameplay.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {

        [Header("Player Config SO")]
        [SerializeField] private PlayerConfig playerConfig;

        [SerializeField] private MonoBehaviour eventBusSource;
        private IEventBus _bus;


        public void SetMax(float v) { playerConfig.SetMaxHealth(Mathf.Max(1f, v)); } 
        public void SetCurrent(float v)
        {
            playerConfig.SetCurrentHealth(Mathf.Clamp(v, 0f, playerConfig.GetMaxHealth()));
            Publish(); //Vigilar si esto causa algun problema
        }

        private void Awake()
        {
            _bus = eventBusSource as IEventBus;
            Publish();
        }

        public bool IsDead => playerConfig.GetCurrentHealth() <= 0f;

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            playerConfig.SetCurrentHealth(playerConfig.GetCurrentHealth() - Mathf.Abs(amount));
            Publish();
        }

        public void Damage(float amount)
        {
            if (IsDead) return;
            playerConfig.SetCurrentHealth(Mathf.Max(0f, playerConfig.GetCurrentHealth() - Mathf.Abs(amount)));
            Publish();
        }






        public void Heal(float amount)
        {
            if (IsDead) return;
            playerConfig.SetCurrentHealth(Mathf.Min(playerConfig.GetMaxHealth(), playerConfig.GetCurrentHealth() + Mathf.Abs(amount)));
            Publish();
        }

        private void Publish() => _bus?.Publish(new HealthChanged(playerConfig.GetCurrentHealth(), playerConfig.GetMaxHealth()));
    }
}
