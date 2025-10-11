using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Data;

namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class EnemyDamageZone : MonoBehaviour
    {
        [SerializeField] private EnemyBase enemy;   // arrastra el EnemyBase del root
        [SerializeField] private float firstHitDelay = 0.5f; // peque�o delay inicial

        [Header("Refs")]
        [SerializeField] private EnemyConfig config;

        private float _cooldownTimer;

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
        }

        private void OnTriggerStay(Collider other)
        {
            if (_cooldownTimer > 0f || enemy == null) return;

            var ph = other.GetComponentInParent<Project.Gameplay.Player.PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(config.GetDamage());                  // PlayerHealth publicar� el evento al HUD
                _cooldownTimer = config.GetAttackCooldown();       // cooldown por golpe
                                                             // al aplicar da�o:
                var bus = FindFirstObjectByType<EventBus>() as IEventBus;
                bus?.Publish(new Project.Core.Events.DTOs.ShowPrompt("You were hit!", 0.8f));

            }
        }
    }
}
