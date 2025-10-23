using System;
using UnityEngine;

namespace Project.Gameplay.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movementSource; // asignar en inspector o GetComponentInParent
        [SerializeField] private string speedParam = "Speed";
        [SerializeField] private string runningParam = "IsRunning";
        [SerializeField, Range(0f, 0.5f)] private float speedDampTime = 0.1f;

        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            if (movementSource == null)
                movementSource = GetComponentInParent<PlayerMovement>();
        }

        private void OnEnable()
        {
            if (movementSource != null)
                movementSource.OnMovementChanged += HandleMovementChanged;
        }

        private void OnDisable()
        {
            if (movementSource != null)
                movementSource.OnMovementChanged -= HandleMovementChanged;
        }

        private void HandleMovementChanged(float speedNormalized, bool isRunning)
        {
            // suavizado para Speed
            _anim.SetFloat(speedParam, speedNormalized, speedDampTime, Time.deltaTime);
            _anim.SetBool(runningParam, isRunning);
        }
    }
}