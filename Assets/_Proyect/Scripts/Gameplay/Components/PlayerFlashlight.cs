using UnityEngine;

namespace Project.Gameplay.Player
{
    public class PlayerFlashlight : MonoBehaviour, IFlashlight
    {
        [SerializeField] private Light lightComp; // asigna el hijo "Flashlight"
        [SerializeField] private float battery = 1f; // 0..1 (stub)
        [SerializeField] private bool isOn = false;

        private void Awake()
        {
            if (lightComp) lightComp.enabled = isOn;
        }

        public void Toggle() => SetOn(!isOn);

        public void SetOn(bool on)
        {
            isOn = on;
            if (lightComp) lightComp.enabled = isOn;
            // TODO: evento BatteryChanged/FlashlightToggled
        }

        public float BatteryNormalized() => Mathf.Clamp01(battery);
    }
}
