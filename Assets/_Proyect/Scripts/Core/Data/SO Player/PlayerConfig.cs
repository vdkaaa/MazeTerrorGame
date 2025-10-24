using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
namespace Project.Data
{
    [CreateAssetMenu(
        fileName = "PlayerConfig",
        menuName = "Configs/Player Config"
    )]
    public class PlayerConfig : ScriptableObject
    {
        #region Vars
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float runSpeed = 6.0f;
        [SerializeField] private float gravity = -9.81f;
   
        [Header("MouseInput")]
        [SerializeField] private float mouseSensitivity = 1.0f;
        [SerializeField] private float pitchClamp = 85f; // límite de mirar arriba/abajo


        [Header("RaycastPlayer")]
        [SerializeField] private float range = 2.5f;
        [SerializeField] private LayerMask interactableMask; // solo Layer Interactable
        [SerializeField] private float promptCooldown = 0.2f;


        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth = 100f;

        [Header("Inventory")]
        [SerializeField] private Dictionary<string, int> _items = new();


        #endregion

        void OnEnable()
        {
            // OnEnable se llama solo cuando el asset se carga, no al iniciar la escena.
            // Es mejor resetear explícitamente el estado al inicio del juego.
            Reset();
        }

        public void Reset()
        {
            currentHealth = maxHealth;
            _items.Clear();
        }

        #region MovementMethods
        public float GetWalkSpeed() => walkSpeed;
        public float GetRunSpeed() => runSpeed;
        public float GetGravity() => gravity;
        #endregion

        #region MouseInputMethods   
        public float GetMouseSensitivity() => mouseSensitivity;
        public float GetPitchClamp() => pitchClamp;
        #endregion

        #region RaycastPlayerMethods
        public float GetRange() => range;
        public LayerMask GetInteractableMask() => interactableMask;
        public float GetPromptCooldown() => promptCooldown;
        #endregion

        #region HealthMethods
        public float GetMaxHealth() => maxHealth;
        public float GetCurrentHealth() => currentHealth;
        public void SetCurrentHealth(float t) => currentHealth = t;
        public void SetMaxHealth(float t) => maxHealth = t;
        #endregion

        #region InventoryMethods
        public Dictionary<string, int> GetItems() => _items;
        public bool HasItem(string id) => Count(id) > 0;
        public int Count(string id) => _items.TryGetValue(id, out int c) ? c : 0;
        #endregion
    }
}
