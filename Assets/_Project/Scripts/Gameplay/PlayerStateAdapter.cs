using UnityEngine;
using Project.Gameplay.Player;
using Project.Core.Events.DTOs;
using System.Collections.Generic;

namespace Project.Core.Services.Save
{
    public class PlayerStateAdapter : MonoBehaviour
    {
        [Header("Refs")]
        private PlayerHealth health;
        private PlayerFlashlight flashlight;
        private PlayerInventory inventory;
        [SerializeField] private Transform playerRoot;
        [SerializeField] private MonoBehaviour eventBusSource; // EventBus para refrescar HUD

        private IEventBus _bus;

        private void Awake()
        {
            if (!playerRoot) playerRoot = transform;
            _bus = eventBusSource as IEventBus;
        }
        private void Start()
        {
            health = GetComponent<PlayerHealth>();
            flashlight = GetComponent<PlayerFlashlight>();
            inventory = GetComponent<PlayerInventory>();
        }
        public GameState Read()
        {
            return new GameState
            {
                playerHealth = health ? health.Current : 100f,
                playerMaxHealth = health ? health.Max : 100f,
                flashlightBattery01 = flashlight ? flashlight.BatteryNormalized() : 1f,
                playerPosition = playerRoot.position,
                playerForward = playerRoot.forward,
                inventoryItems=inventory ? inventory.GetAllItems() : new List<string>(),
            };
        }

        public void Apply(GameState s)
        {
            //PLAYER POS
            if (playerRoot)
            {
                playerRoot.position = s.playerPosition;
                // Orientación simple en Y:
                if (s.playerForward.sqrMagnitude > 0.001f)
                {
                    var f = s.playerForward; f.y = 0f;
                    if (f.sqrMagnitude > 0.001f)
                        playerRoot.rotation = Quaternion.LookRotation(f.normalized, Vector3.up);
                }
            }
            //PLAYER HEALTH 
            if (health)
            {
                health.SetMax(s.playerMaxHealth);   // agrega SetMax en PlayerHealth si aún no existe
                health.SetCurrent(s.playerHealth);  // agrega SetCurrent
                _bus?.Publish(new HealthChanged(health.Current, health.Max));
            }

            //FLASHLIGHT
            if (flashlight)
            {
                flashlight.SetBattery01(s.flashlightBattery01);
                _bus?.Publish(new BatteryChanged(s.flashlightBattery01));
            }

            //PLAYER INVENTORY
            if (inventory != null)
            {
                inventory.LoadFromList(s.inventoryItems);
            }
        }
    }
}
