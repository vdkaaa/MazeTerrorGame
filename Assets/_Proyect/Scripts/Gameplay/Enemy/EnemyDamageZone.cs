using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Data;

namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class EnemyDamageZone : MonoBehaviour
    {
        [SerializeField] private EnemyBase enemy;   // arrastra el EnemyBase del root
        [SerializeField] private float firstHitDelay = 0.5f; // pequeo delay inicial
        [SerializeField] private float attackWindupTime = 1f; // Tiempo antes de hacer daño

        [Header("Refs")]
        [SerializeField] private EnemyConfig config;

        private float _cooldownTimer;
        private bool _isPreparingAttack;
        private float _attackWindupTimer;
        private Project.Gameplay.Player.PlayerHealth _currentTarget;

        private void Awake()
        {
            var col = GetComponent<Collider>();
            col.isTrigger = true;
            _cooldownTimer = firstHitDelay;
            if (!enemy) enemy = GetComponentInParent<EnemyBase>();
        }

        private void Update()
        {
            if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;

            // Si estamos preparando un ataque
            if (_isPreparingAttack)
            {
                _attackWindupTimer -= Time.deltaTime;
                
                // Cuando termina el windup, hacer daño
                if (_attackWindupTimer <= 0f)
                {
                    ExecuteAttack();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_cooldownTimer > 0f || enemy == null || _isPreparingAttack) return;

            var ph = other.GetComponentInParent<Project.Gameplay.Player.PlayerHealth>();
            if (ph != null)
            {
                // Iniciar la preparación del ataque
                _isPreparingAttack = true;
                _attackWindupTimer = attackWindupTime;
                _currentTarget = ph;

                // Aquí podrías activar alguna animación de "preparar ataque"
                var anim = enemy.GetComponent<EnemyAnimator>();
                anim?.PlayAttack(); // Podrías modificar esto para tener una animación de "preparar ataque"
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Si el jugador sale del trigger, cancelar el ataque
            var ph = other.GetComponentInParent<Project.Gameplay.Player.PlayerHealth>();
            if (ph != null && ph == _currentTarget)
            {
                _isPreparingAttack = false;
                _currentTarget = null;
            }
        }

        private void ExecuteAttack()
        {
            if (_currentTarget != null)
            {
                _currentTarget.TakeDamage(config.GetDamage());
                _cooldownTimer = config.GetAttackCooldown();

                var bus = FindFirstObjectByType<EventBus>() as IEventBus;
                bus?.Publish(new Project.Core.Events.DTOs.ShowPrompt("You were hit!", 0.8f));
            }

            // Resetear el estado de ataque
            _isPreparingAttack = false;
            _currentTarget = null;
        }
    }
}
