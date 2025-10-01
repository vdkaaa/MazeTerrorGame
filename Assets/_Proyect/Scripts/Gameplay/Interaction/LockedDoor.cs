using UnityEngine;
using Project.Gameplay.Player;
using Project.Core.Events.DTOs;
using Project.Gameplay.Examples;

namespace Project.Gameplay.Interaction
{
    public class LockedDoor : InteractableBase
    {
        [Header("Lock Settings")]
        [SerializeField] private string requiredKeyId = "RedKey";
        [SerializeField] private DoorJoint door; // reuse your door script (hinge or joint)

        [Header("Prompts")]
        [SerializeField] private string lockedPrompt = "Press E to open (locked)";
        [SerializeField] private string unlockedPrompt = "Press E to open";

        private IEventBus _bus;

        private void Awake()
        {
            _bus = FindFirstObjectByType<EventBus>() as IEventBus;
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
            var inv = interactor.GetComponentInChildren<PlayerInventory>();
            if (!inv)
            {
                _bus?.Publish(new ShowPrompt("No inventory", 1f));
                return;
            }

            if (!inv.HasItem(requiredKeyId))
            {
                _bus?.Publish(new ShowPrompt("The door is locked", 1.5f));
                return;
            }

            // Has the key → unlock and forward interaction to the door
            door.IsLocked = false;
            _bus?.Publish(new ShowPrompt("Door unlocked!", 1.5f));
            door?.Interact(interactor);
        }
    }
}
