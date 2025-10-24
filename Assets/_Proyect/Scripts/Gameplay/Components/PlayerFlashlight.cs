using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Data; // BatteryChanged

namespace Project.Gameplay.Player
{
    public class PlayerFlashlight : MonoBehaviour, IFlashlight
    {
        [Header("Refs")]
        [SerializeField] private Light lightComp;               // Asigna el hijo "Flashlight"
        [SerializeField] private MonoBehaviour eventBusSource;  // Arrastra EventBus de escena

        [Header("Config")]
        [SerializeField] private FlashlightConfig flashlightConfig;


        private IEventBus _bus;

        private void Awake()
        {
            if (flashlightConfig)
            {
                flashlightConfig.Reset();
            }

            if (lightComp) lightComp.enabled = flashlightConfig.IsOn();
            _bus = eventBusSource as IEventBus;
            PublishBattery();
        }

        private void Update()
        {
            if (!flashlightConfig.IsOn()) return;
            if (flashlightConfig.GetBattery() <= 0f) { SetOn(false); return; }

            flashlightConfig.SetBattery(Mathf.Max(0f, flashlightConfig.GetBattery() - flashlightConfig.GetDrainPerSecond() * Time.deltaTime));
            PublishBattery();
        }

        public void Toggle() => SetOn(!flashlightConfig.IsOn());

        public void SetOn(bool on)
        {
            flashlightConfig.setIsOn(on);
            bool isOn = on && flashlightConfig.GetBattery() > 0f;
            if (lightComp) lightComp.enabled = isOn;
            // podr�as publicar un evento FlashlightToggled si quieres
        }

        public float BatteryNormalized() => Mathf.Clamp01(flashlightConfig.GetBattery());

        private void PublishBattery()
        {
            _bus?.Publish(new BatteryChanged(BatteryNormalized()));
        }

        // Helpers dev
        public void AddBattery(float amount01)
        {
            flashlightConfig.SetBattery(flashlightConfig.GetBattery() + amount01);
            Mathf.Clamp01(flashlightConfig.GetBattery() + amount01);
            PublishBattery();
        }


        public void SetBattery01(float t)
        {
            flashlightConfig.SetBattery(t);
            Mathf.Clamp01(t);
            PublishBattery();
        }

    }
}
