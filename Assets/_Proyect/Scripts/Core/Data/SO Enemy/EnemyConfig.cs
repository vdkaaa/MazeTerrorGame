using UnityEngine;

namespace Project.Data
{
    [CreateAssetMenu(
        fileName = "EnemyConfig",
        menuName = "Configs/Enemy Config"
    )]
    public class EnemyConfig : ScriptableObject
    {

        #region Vars
        [Header("Movement")]
        [SerializeField] private float patrolSpeed = 2.2f;
        [SerializeField] private float chaseSpeed = 3.6f;
        [SerializeField] private float stoppingDistance = 1.2f;
        [SerializeField] private float _loseTimer= 1.0f;

        [Header("Detection")]
        [SerializeField] private float loseTargetDelay = 2.0f; // time to return to patrol after losing player

        [Header("Combat")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float attackCooldown = 1.5f;


        #endregion

        #region EnemyMethods
        
        public float GetPatrolSpeed() => patrolSpeed;
        public float GetChaseSpeed() => chaseSpeed;
        public float GetStoppingDistance() => stoppingDistance;
        public float GetLoseTargetDelay() => loseTargetDelay;
        public float GetDamage() => damage;
        public float GetAttackCooldown() => attackCooldown;
        public float GetLoseTimer() => _loseTimer;
        #endregion
    }
}
