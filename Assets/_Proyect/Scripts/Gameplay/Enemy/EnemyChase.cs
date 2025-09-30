using UnityEngine;

namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(EnemyBase))]
    public class EnemyChase : MonoBehaviour
    {
        private EnemyBase _base;

        private void Awake()
        {
            _base = GetComponent<EnemyBase>();
        }

        private void Update()
        {
            if (!_base.HasTarget) return;

            // IA mínima: siempre persigue al Player
            var tpos = _base.Target.position;
            _base.Agent.stoppingDistance = _base.StoppingDistance;
            _base.Agent.SetDestination(tpos);

            // (Opcional) mira hacia el player cuando está cerca
            Vector3 flatDir = (tpos - transform.position);
            flatDir.y = 0f;
            if (flatDir.sqrMagnitude > 0.01f)
            {
                var look = Quaternion.LookRotation(flatDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 5f);
            }
        }
    }
}
