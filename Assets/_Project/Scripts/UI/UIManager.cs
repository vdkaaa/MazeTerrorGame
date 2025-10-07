using UnityEngine;
using Project.Core.Events.DTOs;

namespace Project.UI.HUD
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MonoBehaviour eventBusSource; // arrastra tu EventBus (debe implementar IEventBus)
        [SerializeField] private UIBattery battery;
        [SerializeField] private UIHealth health;
        [SerializeField] private UITime uiTime;
        [SerializeField] private UIPrompt prompt;
        [SerializeField] private UISaveSlotIndicator slotIndicator;
        private IEventBus _bus;

        private void Awake()
        {
            _bus = eventBusSource as IEventBus;
            if (_bus == null)
                Debug.LogError("[UIManager] eventBusSource does not implement IEventBus.");
        }

        private void OnEnable()
        {
            if (_bus == null) return;
            _bus.Subscribe<BatteryChanged>(OnBattery);
            _bus.Subscribe<HealthChanged>(OnHealth);
            _bus.Subscribe<TimeTick>(OnTime);
            _bus.Subscribe<ShowPrompt>(OnPrompt);
            _bus.Subscribe<SaveSlotChanged>(OnSlotChanged);
        }

        private void OnDisable()
        {
            if (_bus == null) return;
            _bus.Unsubscribe<BatteryChanged>(OnBattery);
            _bus.Unsubscribe<HealthChanged>(OnHealth);
            _bus.Unsubscribe<TimeTick>(OnTime);
            _bus.Unsubscribe<ShowPrompt>(OnPrompt);
            _bus.Unsubscribe<SaveSlotChanged>(OnSlotChanged);
        }

        private void OnBattery(BatteryChanged e) => battery?.SetValue(e.normalized);
        private void OnHealth(HealthChanged e) => health?.SetValue(e.Normalized());
        private void OnTime(TimeTick e) => uiTime?.SetClock(e.minutes, e.seconds);
        private void OnPrompt(ShowPrompt e)
        {
            if (e.duration > 0f) prompt?.Show(e.message, e.duration);
            else prompt?.Show(e.message);
        }
        private void OnSlotChanged(SaveSlotChanged e) => slotIndicator?.SetSlot(e.slotId);
    }
}
