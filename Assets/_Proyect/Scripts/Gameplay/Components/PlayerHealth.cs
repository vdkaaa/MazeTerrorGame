using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Data; 
using UnityEngine.SceneManagement;

namespace Project.Gameplay.Player
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {

        [Header("Player Config SO")]
        [SerializeField] private PlayerConfig playerConfig;

        [SerializeField] private MonoBehaviour eventBusSource;
        private bool _isDead = false;
        private IEventBus _bus;


        public void SetMax(float v) { playerConfig.SetMaxHealth(Mathf.Max(1f, v)); } 
        public void SetCurrent(float v)
        {
            playerConfig.SetCurrentHealth(Mathf.Clamp(v, 0f, playerConfig.GetMaxHealth()));
            Publish(); //Vigilar si esto causa algun problema
        }

        private void Awake()
        {
            if (playerConfig)
            {
                playerConfig.Reset();
            }

            _bus = eventBusSource as IEventBus;
            Publish();
        }

        public bool IsDead => playerConfig.GetCurrentHealth() <= 0f;

        public void TakeDamage(float amount)
        {
            if (IsDead) return;
            playerConfig.SetCurrentHealth(playerConfig.GetCurrentHealth() - Mathf.Abs(amount));

            // ¡Aquí publicamos el nuevo evento para el sonido!
            _bus?.Publish(new PlayerTookDamage());
            Publish();
        }

        public void Damage(float amount)
        {
            if (IsDead) return;
            playerConfig.SetCurrentHealth(Mathf.Max(0f, playerConfig.GetCurrentHealth() - Mathf.Abs(amount)));
            Publish();
        }

        void Update()
        {
            if(IsDead)
            {
                // Aquí puedes manejar la lógica de muerte del jugador
                // Por ejemplo, emitir un evento de muerte o reiniciar el nivel
                SceneManager.LoadScene("GameOverScene"); // Carga la escena de Game Over
                
            }
        }




        public void Heal(float amount)
        {
            if (IsDead) return;
            playerConfig.SetCurrentHealth(Mathf.Min(playerConfig.GetMaxHealth(), playerConfig.GetCurrentHealth() + Mathf.Abs(amount)));
            Publish();
        }

        private void Publish() => _bus?.Publish(new HealthChanged(playerConfig.GetCurrentHealth(), playerConfig.GetMaxHealth()));

        public void Revive()
        {
            _isDead = false;
            SetCurrent(playerConfig.GetMaxHealth());
        }
    }
}
