using UnityEngine;
using UnityEngine.AI;

namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(EnemyBase))]
    public class EnemyAnimator : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Animator animator;
        [SerializeField] private EnemyBase enemy;  // raíz del enemigo

        private NavMeshAgent _agent;
        private readonly int SpeedHash = Animator.StringToHash("Speed");
        private readonly int IsChasingHash = Animator.StringToHash("IsChasing");
        private readonly int AttackHash = Animator.StringToHash("Attack");

        private EnemyState _lastState;

        private void Awake()
        {
            if (!enemy) enemy = GetComponent<EnemyBase>();
            if (!_agent) _agent = GetComponent<NavMeshAgent>(); // Buscar directamente
            if (!animator) animator = GetComponentInChildren<Animator>();

            _lastState = enemy.CurrentState;
            
            // Debug mejorado
            Debug.Log($"[EnemyAnimator] Awake - Animator: {(animator != null ? "Found" : "Not Found")}, " +
              $"Agent: {(_agent != null ? "Found" : "Not Found")}, " +
              $"Enemy: {(enemy != null ? "Found" : "Not Found")}");
        }

        private void Update()
        {
            if (!animator || !_agent)
            {
                Debug.LogWarning("[EnemyAnimator] Update - Missing animator or agent!");
                return;
            }

            // Actualizar velocidad al Animator
            float speedPercent = _agent.velocity.magnitude / _agent.speed;
            animator.SetFloat(SpeedHash, speedPercent);
            
            // Debug para velocidad
            //Debug.Log($"[EnemyAnimator] Speed: {speedPercent:F2}");

            // Si cambió el estado, actualizar bool de chase
            if (_lastState != enemy.CurrentState)
            {
                bool isChasing = enemy.CurrentState == EnemyState.Chase;
                animator.SetBool(IsChasingHash, isChasing);
                _lastState = enemy.CurrentState;
                
                // Debug para cambio de estado
                //Debug.Log($"[EnemyAnimator] State changed - IsChasing: {isChasing}, CurrentState: {enemy.CurrentState}");
            }
        }

        /// <summary>
        /// Se puede llamar desde EnemyDamageZone cuando el enemigo golpea al jugador.
        /// </summary>
        public void PlayAttack()
        {
            if (!animator)
            {
                Debug.LogWarning("[EnemyAnimator] PlayAttack - Missing animator!");
                return;
            }
            animator.SetTrigger(AttackHash);
            //Debug.Log("[EnemyAnimator] Attack animation triggered");
        }
    }
}
