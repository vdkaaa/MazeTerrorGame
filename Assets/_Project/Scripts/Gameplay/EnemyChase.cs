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

        }
    }
}
