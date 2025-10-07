using UnityEngine;

namespace Project.Gameplay.Player
{
    /// Puente de entrada de DEV: lee teclado/ratón y alimenta IMovable.
    /// Mañana lo reemplazas por IInputService / Input System, pero hoy te permite caminar ya.
    public class DevPlayerInputBridge : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;  // arrástralo desde el PlayerRoot
        [SerializeField] private float lookMultiplier = 1.0f; // escala del mouse

        private void Reset()
        {
            if (!movement) movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if (!movement) return;

            // Ejes de movimiento (WASD)
            float x = Input.GetAxisRaw("Horizontal"); // A/D  (-1..1)
            float y = Input.GetAxisRaw("Vertical");   // W/S  (-1..1)
            movement.SetMoveInput(x, y);

            // Look (Mouse X/Y)
            float dx = Input.GetAxis("Mouse X") * lookMultiplier;
            float dy = Input.GetAxis("Mouse Y") * lookMultiplier;
            movement.SetLookInput(dx, dy);

            // Correr (LeftShift) — se lo informamos al movimiento
            movement.SetRun(Input.GetKey(KeyCode.LeftShift));

            if (Input.GetKeyDown(KeyCode.F))
            {
                var fl = movement.GetComponent<PlayerFlashlight>();
                if (fl) fl.Toggle();
            }
        }
    }
}
