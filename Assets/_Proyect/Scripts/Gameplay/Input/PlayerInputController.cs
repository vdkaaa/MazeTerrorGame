using UnityEngine;
using Proyect.Inputs;
using Proyect.Core.Services;
    


namespace Project.Gameplay.Player
{
    /// <summary>
    /// Lee el input desde un IInputService y lo traduce en acciones para los componentes del jugador.
    /// </summary>
    public class PlayerInputController : MonoBehaviour
    {
        [Header("Player Components")]
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerFlashlight flashlight;

        private IInputService _inputService;

        private void Reset()
        {
            // Intenta encontrar los componentes en el mismo GameObject o en hijos.
            if (!movement) movement = GetComponentInChildren<PlayerMovement>();
            if (!flashlight) flashlight = GetComponentInChildren<PlayerFlashlight>();
        }

        private void Awake()
        {
            // Aquí deberías obtener el servicio desde tu Service Locator o Inyector de Dependencias.
            // Por ahora, lo instanciamos directamente para el ejemplo.
            _inputService = new InputService();
        }

        private void Update()
        {
            if (movement != null)
            {
                movement.SetMoveInput(_inputService.MoveInput.x, _inputService.MoveInput.y);
                movement.SetLookInput(_inputService.LookInput.x, _inputService.LookInput.y);
                movement.SetRun(_inputService.IsRunning);
            }

            if (flashlight != null && _inputService.ToggleFlashlightPressed) flashlight.Toggle();
        }
    }
}
