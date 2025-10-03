using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Gameplay.Interaction;

namespace Project.Gameplay.Player
{
    public class AimDetector : MonoBehaviour
    {
        [SerializeField] private float requiredHold = 1.2f; // segundos para examinar
        [SerializeField] private LayerMask inspectableMask;
        [SerializeField] private float maxDistance = 4f;
        [SerializeField] private MonoBehaviour eventBusSource;
        private IEventBus _bus;

        private IInteractable _current;
        private float _holdTimer;

        private void Awake()
        {
            _bus = eventBusSource as IEventBus;
        }

        private void Update()
        {
            // ray desde cámara
            var cam = Camera.main;
            if (!cam) return;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, maxDistance, inspectableMask))
            {
                var interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    if (_current != interactable) { _current = interactable; _holdTimer = 0f; }
                    _holdTimer += Time.deltaTime;
                    _bus?.Publish(new ShowPrompt(interactable.Prompt(), 0f)); // persist while aiming

                    if (_holdTimer >= requiredHold)
                    {
                        // comanda al interactable que sea "examined"
                        (interactable as IInspectable)?.OnExamined(gameObject);
                        _holdTimer = 0f;
                    }
                    return;
                }
            }

            // no apunta a nada
            _current = null;
            _holdTimer = 0f;
        }
    }
}
