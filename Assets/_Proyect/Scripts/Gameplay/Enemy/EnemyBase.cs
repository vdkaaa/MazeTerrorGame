using UnityEngine;
using UnityEngine.AI;
using Project.Data;


namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBase : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private EnemyConfig config; 
        public NavMeshAgent Agent { get; private set; }
        public EnemyState CurrentState { get; private set; } = EnemyState.Idle;
        public Transform Target { get; private set; }

        private float _loseTimer;

        public EnemyConfig GetConfig() => config;
        
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = true;
            Agent.updateUpAxis = true;

            if (Target == null)
            {
                var ph = Object.FindFirstObjectByType<Project.Gameplay.Player.PlayerHealth>();
                if (ph) Target = ph.transform;
            }
        }



        public void SetState(EnemyState s)
        {
            if (CurrentState == s) return;
            CurrentState = s;
            // speed per state
            Agent.speed = (s == EnemyState.Chase) ? config.GetChaseSpeed() : config.GetPatrolSpeed();
        }

        public void ResetLoseTimer() => _loseTimer = config.GetLoseTargetDelay();

        public bool TickLoseTimer(float dt)
        {
            if (CurrentState != EnemyState.Chase) return false;
            _loseTimer -= dt;
            return _loseTimer <= 0f;
        }
    }
}
