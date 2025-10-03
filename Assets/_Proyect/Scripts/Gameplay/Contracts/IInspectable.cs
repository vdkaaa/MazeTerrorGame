using Project.Gameplay.Interaction;
using UnityEngine;

public interface IInspectable : IInteractable
{
    void OnExamined(GameObject examiner); // Quién interactúa (Player u otro)
}


