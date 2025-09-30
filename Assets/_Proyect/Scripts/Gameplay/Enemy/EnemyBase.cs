using UnityEngine;
using UnityEngine.AI;

namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyBase : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform target; // Player (si no asignas, lo encuentra en Awake)

        [Header("Movement")]
        [SerializeField] private float chaseSpeed = 3.2f;
        [SerializeField] private float stoppingDistance = 1.2f;

        [Header("Combat")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float attackCooldown = 1.5f;

        [Header("Detection")]
        [SerializeField] private float detectionRange = 50f; // por ahora siempre te ve

        public NavMeshAgent Agent { get; private set; }
        public Transform Target => target;
        public float Damage => damage;
        public float AttackCooldown => attackCooldown;
        public float StoppingDistance => stoppingDistance;
        public float DetectionRange => detectionRange;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.speed = chaseSpeed;
            Agent.stoppingDistance = stoppingDistance;
            Agent.updateRotation = true;
            Agent.updateUpAxis = true;

            if (target == null)
            {
                // Busca un PlayerHealth en escena y toma su transform raíz
                var ph = Object.FindFirstObjectByType<Project.Gameplay.Player.PlayerHealth>();
                if (ph) target = ph.transform;
            }
        }

        public bool HasTarget => target != null;
    }
}
