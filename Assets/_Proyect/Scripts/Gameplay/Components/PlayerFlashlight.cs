using UnityEngine;
using Project.Core.Events.DTOs; // BatteryChanged

namespace Project.Gameplay.Player
{
    public class PlayerFlashlight : MonoBehaviour, IFlashlight
    {
        [Header("Refs")]
        [SerializeField] private Light lightComp;               // Asigna el hijo "Flashlight"
        [SerializeField] private MonoBehaviour eventBusSource;  // Arrastra EventBus de escena

        [Header("Config")]
        [SerializeField, Range(0f, 1f)] private float battery = 1f; // 0..1
        [SerializeField] private float drainPerSecond = 0.05f;     // puedes reemplazar por SO luego
        [SerializeField] private bool isOn = false;

        private IEventBus _bus;

        private void Awake()
        {
            if (lightComp) lightComp.enabled = isOn;
            _bus = eventBusSource as IEventBus;
            PublishBattery();
        }

        private void Update()
        {
            if (!isOn) return;
            if (battery <= 0f) { SetOn(false); return; }

            battery = Mathf.Max(0f, battery - drainPerSecond * Time.deltaTime);
            PublishBattery();
        }

        public void Toggle() => SetOn(!isOn);

        public void SetOn(bool on)
        {
            isOn = on && battery > 0f;
            if (lightComp) lightComp.enabled = isOn;
            // podrías publicar un evento FlashlightToggled si quieres
        }

        public float BatteryNormalized() => Mathf.Clamp01(battery);

        private void PublishBattery()
        {
            _bus?.Publish(new BatteryChanged(BatteryNormalized()));
        }

        // Helpers dev
        public void AddBattery(float amount01)
        {
            battery = Mathf.Clamp01(battery + amount01);
            PublishBattery();
        }
    }
}
