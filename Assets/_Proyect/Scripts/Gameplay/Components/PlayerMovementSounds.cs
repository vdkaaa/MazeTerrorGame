// path: Assets/_Proyect/Scripts/Gameplay/Player/PlayerMovementSounds.cs
using UnityEngine;

namespace Project.Gameplay.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerMovementSounds : MonoBehaviour
    {
        [Header("Audio Clips")]
        [Tooltip("Sonido continuo que se reproduce al caminar.")]
        [SerializeField] private AudioClip walkingSound;
        [Tooltip("Sonido continuo que se reproduce al correr.")]
        [SerializeField] private AudioClip runningSound;

        // Referencias a otros componentes del jugador
        private CharacterController _characterController;
        private PlayerMovement _playerMovement;
        private AudioSource _audioSource;        

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _characterController = GetComponentInParent<CharacterController>();
            _playerMovement = GetComponentInParent<PlayerMovement>();

            if (_characterController == null || _playerMovement == null)
            {
                Debug.LogError("PlayerMovementSounds no pudo encontrar CharacterController o PlayerMovement en el padre.", this);
                enabled = false; // Desactivamos el script si no encuentra lo que necesita.
            }

            // Nos aseguramos de que el AudioSource esté configurado para hacer bucle (loop).
            _audioSource.loop = true;
        }

        private void Update()
        {
            bool isGrounded = _characterController.isGrounded;
            // Calculamos la velocidad horizontal para saber si se está moviendo
            Vector3 horizontalVelocity = new Vector3(_characterController.velocity.x, 0, _characterController.velocity.z);
            bool isMoving = horizontalVelocity.magnitude > 0.1f;

            if (isGrounded && isMoving)
            {
                // El jugador se está moviendo en el suelo.
                bool isRunning = _playerMovement.IsRunning();
                AudioClip clipToPlay = isRunning ? runningSound : walkingSound;

                // Si el clip que debe sonar no es el que está puesto, o si no está sonando, lo reproducimos.
                if (_audioSource.clip != clipToPlay || !_audioSource.isPlaying)
                {
                    _audioSource.clip = clipToPlay;
                    if (clipToPlay != null)
                    {
                        _audioSource.Play();
                    }
                }
            }
            else
            {
                // El jugador está quieto o en el aire, así que detenemos el sonido.
                _audioSource.Stop();
            }
        }
    }
}
