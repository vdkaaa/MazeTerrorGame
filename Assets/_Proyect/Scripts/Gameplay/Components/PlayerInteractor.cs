using UnityEngine;

namespace Project.Gameplay.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private float range = 2.0f;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private Transform rayOrigin; // asigna Camera/main

        public bool TryGetInteractable(out IInteractable interactable)
        {
            interactable = null;
            Transform origin = rayOrigin ? rayOrigin : transform;
            if (Physics.Raycast(origin.position, origin.forward, out var hit, range, interactableMask))
            {
                interactable = hit.collider.GetComponentInParent<IInteractable>();
                return interactable != null;
            }
            return false;
        }

        public void TryInteract()
        {
            if (TryGetInteractable(out var it)) it.Interact();
        }
    }
}
