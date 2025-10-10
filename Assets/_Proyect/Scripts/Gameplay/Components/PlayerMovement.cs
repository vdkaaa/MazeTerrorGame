using Project.Data;
using UnityEngine;

namespace Project.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IMovable
    {
        [Header("Player Config SO")]
        [SerializeField] private PlayerConfig playerConfig;


        [Header("CharacterController")]
        private CharacterController _cc;
        private Transform _cameraRig;

        private Vector2 _move;       // input x,y
        private Vector2 _look;       // mouse dx,dy
        private bool _isRunning;     // lo setea el bridge
        private float _verticalVelocity;
        private float _pitch;        // acumulado para cámara (X)

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();
            _cameraRig = transform.Find("CameraRig");
            if (_cameraRig == null) Debug.LogWarning("[PlayerMovement] CameraRig no encontrado (esperado como hijo).");
            // Opcional: bloquear el cursor para mirar con mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // IMovable
        public void SetMoveInput(float x, float y) => _move = new Vector2(x, y);
        public void SetLookInput(float dx, float dy) => _look = new Vector2(dx, dy);
        public void SetRun(bool isRunning) => _isRunning = isRunning;

        // Método para exponer la configuración
        public PlayerConfig GetPlayerConfig() => playerConfig;

        private void Update()
        {
            // Rotación horizontal del cuerpo (yaw)
            transform.Rotate(Vector3.up, _look.x * playerConfig.GetMouseSensitivity());

            // Rotación vertical de la cámara (pitch, con clamp)
            if (_cameraRig)
            {
                _pitch = Mathf.Clamp(_pitch - _look.y * playerConfig.GetMouseSensitivity(), -playerConfig.GetPitchClamp(), playerConfig.GetPitchClamp());
                _cameraRig.localEulerAngles = new Vector3(_pitch, 0f, 0f);
            }

            // Movimiento en plano
            Vector3 dir = (transform.right * _move.x + transform.forward * _move.y).normalized;
            float speed = _isRunning ? playerConfig.GetRunSpeed() : playerConfig.GetWalkSpeed();

            // Gravedad simple
            if (_cc.isGrounded && _verticalVelocity < 0f) _verticalVelocity = -1f;
            _verticalVelocity += playerConfig.GetGravity() * Time.deltaTime;

            Vector3 vel = dir * speed + Vector3.up * _verticalVelocity;
            _cc.Move(vel * Time.deltaTime);
        }
    }
}
