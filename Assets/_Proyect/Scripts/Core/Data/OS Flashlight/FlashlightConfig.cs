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
        public float intensity = 1200f;       // l�menes o unidad relativa
        public float angle = 45f;             // �ngulo del cono
        public float drainPerSecond = 0.05f;  // consumo de bater�a por segundo
    }
}
