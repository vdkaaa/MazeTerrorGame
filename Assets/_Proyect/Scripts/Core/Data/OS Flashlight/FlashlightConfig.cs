using UnityEngine;

namespace Project.Data
{
    [CreateAssetMenu(
        fileName = "FlashlightConfig",
        menuName = "Configs/Flashlight Config"
    )]
    public class FlashlightConfig : ScriptableObject
    {

        #region Vars
        [Header("Flashlight")]
        [SerializeField, Range(0f, 1f)] private float battery = 1f; // 0..1
        [SerializeField] private float drainPerSecond = 0.05f;     // puedes reemplazar por SO luego
        [SerializeField] private bool isOn = false;

        #endregion

        #region FlashlightMethods
        public float GetBattery() => battery;
        public void SetBattery(float t) => battery = t;
        public float GetDrainPerSecond() => drainPerSecond;



        public bool setIsOn(bool t) => isOn = t;    
        public bool IsOn() => isOn;
        #endregion
    }
}
