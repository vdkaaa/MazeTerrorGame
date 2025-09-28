namespace Project.Gameplay.Interaction
{
    public interface IInteractable
    {
        string Prompt();                          // Texto para HUD
        void Interact(UnityEngine.GameObject interactor); // Quién interactúa (Player u otro)
    }
}
