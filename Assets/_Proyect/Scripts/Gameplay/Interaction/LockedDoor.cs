using UnityEngine;
using Project.Gameplay.Player;
using Project.Core.Events.DTOs;
using Project.Data; 
using Project.Gameplay.Examples;

 
namespace Project.Gameplay.Interaction
{
    public class LockedDoor : InteractableBase
    {
        [Header("Lock Settings")]
        [SerializeField] private string requiredKeyId = "RedKey";
        [SerializeField] private DoorJoint door; // reuse your door script (hinge or joint)

        [Header("Events & Prompts")]
        [SerializeField] private string lockedPrompt = "Press E to open (locked)";
        [SerializeField] private string unlockedPrompt = "Press E to open";
        [SerializeField] private MonoBehaviour eventBusSource;

        private IEventBus _bus;

        private void Awake()
        {
            _bus = eventBusSource as IEventBus;
            if (!door) door = GetComponent<DoorJoint>();
        }

        // <<< KEY CHANGE: dynamic prompt based on state >>>
        public override string Prompt()
        {
            // If door reference is missing, fall back gracefully
            if (!door) return lockedPrompt;
            return door.IsLocked ? lockedPrompt : unlockedPrompt;
        }

        public override void Interact(GameObject interactor)
        {
            var playerMovement = interactor.GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("El objeto que interactúa no tiene PlayerMovement.", this);
                return;
            }

            PlayerConfig config = playerMovement.GetPlayerConfig(); // Necesitarás añadir este método a PlayerMovement
            if (config == null)
            {
                Debug.LogError("No se pudo obtener PlayerConfig desde PlayerMovement.", this);
                return;
            }

            // Paso 2: Usar el PlayerConfig para verificar si tiene el item.
            if (!config.HasItem(requiredKeyId))
            {
                _bus?.Publish(new ShowPrompt("The door is locked", 1.5f));
                return;
            }

            // Has the key → unlock and forward interaction to the door
            door.IsLocked = false;
            _bus?.Publish(new ShowPrompt("Door unlocked!", 1.5f));
            door.Interact(interactor); // Ya no es necesario el '?' porque si no existe, el Awake ya lo habría asignado.
        }
    }
}
