using UnityEngine;
using Project.Gameplay.Interaction;

namespace Project.Gameplay.Examples
{
    [RequireComponent(typeof(HingeJoint))]
    public class DoorJoint : InteractableBase
    {
        [Header("Config")]
        [SerializeField] private float openVelocity = 90f;   // deg/sec to open
        [SerializeField] private float closeVelocity = -90f; // deg/sec to close
        [SerializeField] private float motorForce = 100f;    // motor force

        private HingeJoint hinge;
        private bool isOpen = false;
        public bool IsLocked;

        private void Awake()
        {
            hinge = GetComponent<HingeJoint>();

            if (string.IsNullOrEmpty(prompt))
                prompt = "Press E to open/close";

            // Start without motor
            hinge.useMotor = false;

            if (!hinge.useLimits)
                Debug.LogWarning("[DoorJoint] It is recommended to enable 'Use Limits' in HingeJoint.");

            IsLocked = true;
        }

        private void SetMotor(float velocity)
        {
            var motor = hinge.motor;
            motor.force = motorForce;
            motor.targetVelocity = velocity;
            hinge.motor = motor;
            hinge.useMotor = true;
        }

        public override void Interact(GameObject interactor)
        {
            Debug.Log(IsLocked);
            if (IsLocked) return;     // ignore until unlocked
            isOpen = !isOpen;

            if (isOpen)
            {
                SetMotor(openVelocity);
            }
            else
            {
                SetMotor(closeVelocity);
            }
        }
    }
}
