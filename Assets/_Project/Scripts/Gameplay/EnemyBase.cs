using UnityEngine;
using UnityEngine.AI;

namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBase : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform target; // Player transform (auto-discover if null)
        public Transform Target => target;

        [Header("Movement")]
        public float patrolSpeed = 2.2f;
        public float chaseSpeed = 3.6f;
        public float stoppingDistance = 1.2f;

        [Header("Detection")]
        public float loseTargetDelay = 2.0f; // time to return to patrol after losing player

        [Header("Combat")]
        private float damage = 10f;
        private float attackCooldown = 1.5f;

        public NavMeshAgent Agent { get; private set; }
        public EnemyState CurrentState { get; private set; } = EnemyState.Idle;
        public float Damage { get => damage; }
        public float AttackCooldown { get => attackCooldown; }

        float _loseTimer;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = true;
            Agent.updateUpAxis = true;

            if (target == null)
            {
                var ph = Object.FindFirstObjectByType<Project.Gameplay.Player.PlayerHealth>();
                if (ph) target = ph.transform;
            }
        }



        public void SetState(EnemyState s)
        {
            if (CurrentState == s) return;
            CurrentState = s;
            // speed per state
            Agent.speed = (s == EnemyState.Chase) ? chaseSpeed : patrolSpeed;
        }

        public void ResetLoseTimer() => _loseTimer = loseTargetDelay;

        public bool TickLoseTimer(float dt)
        {
            if (CurrentState != EnemyState.Chase) return false;
            _loseTimer -= dt;
            return _loseTimer <= 0f;
        }
    }
}
