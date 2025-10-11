
using UnityEngine;

public interface IInputService 
{
    Vector2 MoveInput { get; }
    Vector2 LookInput { get; }
    bool IsRunning { get; }

    bool ToggleFlashlightPressed { get; }
}
