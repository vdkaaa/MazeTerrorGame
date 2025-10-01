using UnityEngine;
using Project.Gameplay.Interaction;
using Project.Gameplay.Player;
using Project.Core.Events.DTOs;

namespace Project.Gameplay.Interaction
{
    public class KeyItemPickup : InteractableBase
    {
        [SerializeField] private string keyId = "RedKey";
        [SerializeField] private GameObject visuals;

        public override void Interact(GameObject interactor)
        {
            var inv = interactor.GetComponentInChildren<PlayerInventory>();
            if (!inv) return;

            inv.AddItem(keyId);

            // Optional feedback
            var bus = FindFirstObjectByType<EventBus>() as IEventBus;
            bus?.Publish(new ShowPrompt($"Picked up {keyId}", 1.5f));

            if (visuals) visuals.SetActive(false);
            Destroy(gameObject);
        }

        private void Reset()
        {
            if (string.IsNullOrEmpty(prompt))
                prompt = "Press E to pick up key";
        }
    }
}
