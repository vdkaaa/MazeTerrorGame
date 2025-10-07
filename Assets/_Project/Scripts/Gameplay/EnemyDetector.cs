using UnityEngine;

namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyDetector : MonoBehaviour
    {
        [SerializeField] private EnemyBase enemy;    // assign root
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private bool requireLineOfSight = false;
        [SerializeField] private Transform eye;      // optional; ray origin for LOS

        public bool PlayerInRange { get; private set; }
        Transform _player;

        private void Reset()
        {
            var col = GetComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 8f;
        }

        private void Awake()
        {
            if (!enemy) enemy = GetComponentInParent<EnemyBase>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsPlayer(other))
            {
                _player = other.transform;
                PlayerInRange = true;
                enemy?.ResetLoseTimer();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!IsPlayer(other)) return;
            PlayerInRange = HasLOS(other.transform);
            if (PlayerInRange) enemy?.ResetLoseTimer();
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsPlayer(other))
            {
                PlayerInRange = false;
                _player = null;
            }
        }

        bool IsPlayer(Collider c)
        {
            int mask = 1 << c.gameObject.layer;
            return (playerMask.value & mask) != 0;
        }

        bool HasLOS(Transform t)
        {
            if (!requireLineOfSight) return true;
            var origin = eye ? eye.position : transform.position + Vector3.up * 3.6f;
            var dir = (t.position + Vector3.up * 1.0f) - origin;
            if (Physics.Raycast(origin, dir.normalized, out var hit, dir.magnitude))
                return hit.transform.IsChildOf(t); // clear if first hit is player
            return false;
        }
    }
}
