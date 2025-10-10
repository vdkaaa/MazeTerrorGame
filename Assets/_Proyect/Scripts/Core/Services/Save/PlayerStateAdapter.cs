using UnityEngine;
using Project.Gameplay.Player;
using Project.Core.Events.DTOs;
using System.Collections.Generic;
using Project.Data; 

namespace Project.Core.Services.Save
{
    public class PlayerStateAdapter : MonoBehaviour
    {
        [Header("Refs")]

        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private FlashlightConfig flashlightConfig;

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
            inventory = GetComponent<PlayerInventory>();
        }
        public GameState Read()
        {
            return new GameState
            {
                playerHealth = playerConfig.GetCurrentHealth(),
                playerMaxHealth = playerConfig.GetMaxHealth(),
                flashlightBattery01 = flashlightConfig.GetBattery(), // Asumiendo que GetBattery() devuelve un valor normalizado (0-1)
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
                // Orientaci�n simple en Y:
                if (s.playerForward.sqrMagnitude > 0.001f)
                {
                    var f = s.playerForward; f.y = 0f;
                    if (f.sqrMagnitude > 0.001f)
                        playerRoot.rotation = Quaternion.LookRotation(f.normalized, Vector3.up);
                }
            }
            //PLAYER HEALTH 
            if (playerConfig)
            {
                playerConfig.SetMaxHealth(s.playerMaxHealth);
                playerConfig.SetCurrentHealth(s.playerHealth);
                _bus?.Publish(new HealthChanged(s.playerHealth, s.playerMaxHealth));
            }

            //FLASHLIGHT
            if (flashlightConfig)
            {
                flashlightConfig.SetBattery(s.flashlightBattery01);
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
