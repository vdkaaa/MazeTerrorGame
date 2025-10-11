using UnityEngine;
using Project.Core.Events.DTOs; // for ShowPrompt

namespace Project.Gameplay.Enemy
{
    [RequireComponent(typeof(EnemyBase))]
    public class EnemyChaseFSM : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private EnemyDetector detector;     // child with SphereCollider
        [SerializeField] private MonoBehaviour eventBusSource;

        EnemyBase _base;
        IEventBus _bus;

        private void Awake()
        {
            _base = GetComponent<EnemyBase>();
            _bus = eventBusSource as IEventBus;
            if (!detector) detector = GetComponentInChildren<EnemyDetector>();
        }

        private void Update()
        {
            if (!_base.Target) return;

            // Chase transition
            if (detector && detector.PlayerInRange)
            {
                if (_base.CurrentState != EnemyState.Chase)
                {
                    _base.SetState(EnemyState.Chase);
                    _bus?.Publish(new ShowPrompt("Enemy spotted you!", 1.0f));
                }
                DoChase();
                return;
            }

            // If in chase but lost sight for a while → back to patrol
            if (_base.CurrentState == EnemyState.Chase)
            {
                if (_base.TickLoseTimer(Time.deltaTime))
                    _base.SetState(EnemyState.Patrol);
                else
                    DoChase();
            }
        }

        void DoChase()
        {
            _base.Agent.stoppingDistance = _base.GetConfig().GetStoppingDistance();
            _base.Agent.SetDestination(_base.Target.position);
        }
    }
}
