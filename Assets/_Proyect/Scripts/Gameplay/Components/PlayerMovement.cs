using UnityEngine;

namespace Project.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IMovable
    {
        [Header("Tuning")]
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float runSpeed = 6.0f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float mouseSensitivity = 1.0f;

        private CharacterController _cc;
        private Transform _cameraRig;

        private Vector2 _move; // input x,y
        private Vector2 _look; // mouse dx,dy
        private float _verticalVelocity;

        private void Awake()
        {
            _cc = GetComponent<CharacterController>();
            _cameraRig = transform.Find("CameraRig");
        }

        // IMovable
        public void SetMoveInput(float x, float y) => _move = new Vector2(x, y);
        public void SetLookInput(float dx, float dy) => _look = new Vector2(dx, dy);

        private void Update()
        {
            // Rotación horizontal con mouse
            transform.Rotate(Vector3.up, _look.x * mouseSensitivity);

            // Movimiento en plano
            Vector3 dir = (transform.right * _move.x + transform.forward * _move.y);
            float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            // Gravedad simple
            if (_cc.isGrounded && _verticalVelocity < 0) _verticalVelocity = -1f;
            _verticalVelocity += gravity * Time.deltaTime;

            Vector3 vel = dir * speed + Vector3.up * _verticalVelocity;
            _cc.Move(vel * Time.deltaTime);
        }
    }
}
