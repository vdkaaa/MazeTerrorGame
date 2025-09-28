using UnityEngine;
using Project.Gameplay.Interaction;

namespace Project.Gameplay.Examples
{
    [RequireComponent(typeof(HingeJoint))]
    public class DoorJoint : InteractableBase
    {
        [Header("Config")]
        [SerializeField] private float openVelocity = 90f;   // grados/seg para abrir
        [SerializeField] private float closeVelocity = -90f; // grados/seg para cerrar
        [SerializeField] private float motorForce = 100f;    // fuerza del motor

        private HingeJoint hinge;
        private bool isOpen = false;

        private void Awake()
        {
            hinge = GetComponent<HingeJoint>();

            if (string.IsNullOrEmpty(prompt))
                prompt = "Press E to open/close";

            // Config inicial: sin motor
            hinge.useMotor = false;

            // Opcional: asegura que tiene límites
            if (!hinge.useLimits)
                Debug.LogWarning("[DoorJoint] Te recomiendo activar 'Use Limits' en el HingeJoint");
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
            isOpen = !isOpen;

            if (isOpen)
            {
                // Empuja hacia el ángulo máximo definido en el Joint
                SetMotor(openVelocity);
            }
            else
            {
                // Empuja hacia el ángulo mínimo definido en el Joint
                SetMotor(closeVelocity);
            }
        }
    }
}
