using UnityEngine;
using Proyect.Inputs;

namespace Proyect.Core.Services
{
    public class InputService : IInputService
    {
        private readonly PlayerControls _playerControls;

        public Vector2 MoveInput => _playerControls.Gameplay.Move.ReadValue<Vector2>();
        public Vector2 LookInput => _playerControls.Gameplay.Look.ReadValue<Vector2>();
        public bool IsRunning => _playerControls.Gameplay.Run.IsPressed();
        public bool ToggleFlashlightPressed => _playerControls.Gameplay.ToggleFlashlight.WasPressedThisFrame();

        public InputService()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();
        }
    }
}
