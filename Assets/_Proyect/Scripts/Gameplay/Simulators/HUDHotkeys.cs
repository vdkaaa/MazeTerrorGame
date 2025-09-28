using UnityEngine;
using Project.Core.Events.DTOs;

public class HUDHotkeys : MonoBehaviour
{
    [SerializeField] private MonoBehaviour eventBusSource;
    [SerializeField] private Project.Gameplay.Player.PlayerHealth playerHealth;
    [SerializeField] private Project.Gameplay.Player.PlayerFlashlight playerFlashlight;

    private IEventBus _bus;
    private void Awake() { _bus = eventBusSource as IEventBus; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) playerHealth?.TakeDamage(10f);
        if (Input.GetKeyDown(KeyCode.J)) playerHealth?.Heal(10f);
        if (Input.GetKeyDown(KeyCode.B)) playerFlashlight?.AddBattery(0.1f);
        if (Input.GetKeyDown(KeyCode.P)) _bus?.Publish(new ShowPrompt("Picked up battery", 1.2f));
    }
}
