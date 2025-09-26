using UnityEngine;

namespace Project.Data
{
    [CreateAssetMenu(
        fileName = "FlashlightConfig",
        menuName = "Configs/Flashlight Config"
    )]
    public class FlashlightConfig : ScriptableObject
    {
        [Header("Flashlight")]
        public float intensity = 1200f;       // lúmenes o unidad relativa
        public float angle = 45f;             // ángulo del cono
        public float drainPerSecond = 0.05f;  // consumo de batería por segundo
    }
}
