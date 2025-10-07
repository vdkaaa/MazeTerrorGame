using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(EnemyBase))]
    public class EnemyPatrol : MonoBehaviour
    {
        [SerializeField] private List<Transform> points = new();
        [SerializeField] private float waypointTolerance = 0.5f;
        [SerializeField] private bool loop = true;

        private EnemyBase _base;
        int _index;

        public void SetPoints(List<Transform> pts) { points = pts; _index = 0; }

        private void Awake() { 
            _base = GetComponent<EnemyBase>(); 

        }

        private void Start()
        {
            if (_base == null) _base = GetComponent<EnemyBase>();
            if (_base != null && points.Count > 0)
                _base.SetState(EnemyState.Patrol);
        }

        private void Update()
        {
            if (_base.CurrentState != EnemyState.Patrol) return;
            if (points.Count == 0) return;

            var tgt = points[_index].position;
            _base.Agent.stoppingDistance = 0f;
            _base.Agent.SetDestination(tgt);

            if (!_base.Agent.pathPending && _base.Agent.remainingDistance <= Mathf.Max(0.1f, waypointTolerance))
            {
                _index++;
                if (_index >= points.Count) _index = loop ? 0 : points.Count - 1;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < points.Count; i++)
            {
                if (!points[i]) continue;
                Gizmos.DrawSphere(points[i].position, 0.1f);
                if (i + 1 < points.Count && points[i + 1])
                    Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
        }
#endif
    }
}
