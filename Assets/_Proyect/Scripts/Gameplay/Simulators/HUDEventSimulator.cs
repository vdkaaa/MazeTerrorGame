using UnityEngine;
using Project.Core.Events.DTOs;

public class HUDEventSimulator : MonoBehaviour
{
    [SerializeField] private MonoBehaviour eventBusSource; // tu EventBus
    private IEventBus _bus;
    private float _t;
    private int _sec;
    private int _min;

    private void Awake()
    {
        _bus = eventBusSource as IEventBus;
        if (_bus == null) Debug.LogError("[HUDEventSimulator] eventBusSource does not implement IEventBus.");
    }

    private void Update()
    {
        if (_bus == null) return;

        // Batería oscila (0.25..1.0)
        _t += Time.deltaTime * 0.5f;
        float battery = 0.625f + 0.375f * Mathf.Sin(_t);
        _bus.Publish(new BatteryChanged(Mathf.Clamp01(battery)));

        // Tiempo (simple mm:ss)
        if (Time.frameCount % 60 == 0)
        {
            _sec++;
            if (_sec >= 60) { _sec = 0; _min++; }
            _bus.Publish(new TimeTick(_min, _sec));
        }

        // Prompt cada ~5 s
        if (Mathf.FloorToInt(Time.time) % 5 == 0 && Time.frameCount % 60 == 0)
        {
            _bus.Publish(new ShowPrompt("Press E to interact", 1.2f));
        }

        // Salud baja y sube suave (demo)
        float hp = 75f + 25f * Mathf.Sin(_t * 0.7f);
        _bus.Publish(new HealthChanged(hp, 100f));
    }
}
