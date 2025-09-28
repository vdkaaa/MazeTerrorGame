using UnityEngine;
using Project.Gameplay.Interaction;

namespace Project.Gameplay.Examples
{
    public class BatteryPickup : InteractableBase
    {
        [SerializeField, Range(0f, 1f)] private float addAmount01 = 0.25f;
        [SerializeField] private GameObject visuals;

        public override void Interact(GameObject interactor)
        {
            var flashlight = interactor.GetComponentInChildren<Project.Gameplay.Player.PlayerFlashlight>();
            if (!flashlight) return;

            flashlight.AddBattery(addAmount01);
            if (visuals) visuals.SetActive(false);
            Destroy(gameObject);
        }

        private void Reset()
        {
            if (string.IsNullOrEmpty(prompt))
                prompt = "Press E to pick up battery";
        }
    }
}
