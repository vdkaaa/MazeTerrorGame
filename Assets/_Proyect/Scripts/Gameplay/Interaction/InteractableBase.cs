using UnityEngine;

namespace Project.Gameplay.Interaction
{
    public abstract class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] protected string prompt = "Press E to interact";

        public virtual string Prompt() => prompt;

        // Obliga a las subclases a implementar su acción
        public abstract void Interact(GameObject interactor);
    }
}
