using UnityEngine;
using Project.Core.Events.DTOs;

public class HUDHotkeys : MonoBehaviour
{
    [SerializeField] private MonoBehaviour eventBusSource; // arrastra EventBus
    private IEventBus _bus;
    private float battery = 1f;
    private float hp = 100f;
    private int min, sec;

    void Awake() { _bus = eventBusSource as IEventBus; }

    void Update()
    {
        if (_bus == null) return;

        if (Input.GetKeyDown(KeyCode.B)) { battery = Mathf.Clamp01(battery - 0.1f); _bus.Publish(new BatteryChanged(battery)); }
        if (Input.GetKeyDown(KeyCode.N)) { battery = Mathf.Clamp01(battery + 0.1f); _bus.Publish(new BatteryChanged(battery)); }

        if (Input.GetKeyDown(KeyCode.H)) { hp = Mathf.Max(0, hp - 10f); _bus.Publish(new HealthChanged(hp, 100f)); }
        if (Input.GetKeyDown(KeyCode.J)) { hp = Mathf.Min(100f, hp + 10f); _bus.Publish(new HealthChanged(hp, 100f)); }

        if (Input.GetKeyDown(KeyCode.T)) { sec++; if (sec >= 60) { sec = 0; min++; } _bus.Publish(new TimeTick(min, sec)); }

        if (Input.GetKeyDown(KeyCode.P)) { _bus.Publish(new ShowPrompt("Picked up a battery", 1.5f)); }
    }
}
