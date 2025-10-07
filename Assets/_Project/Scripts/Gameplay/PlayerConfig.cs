using UnityEngine;

namespace Project.Data
{
    [CreateAssetMenu(
        fileName = "PlayerConfig",
        menuName = "Configs/Player Config"
    )]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Movement")]
        public float walkSpeed = 3.5f;
        public float runSpeed = 6.0f;
    }
}
