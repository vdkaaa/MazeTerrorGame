using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Gameplay.Interaction;

namespace Project.Gameplay.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Raycast")]
        [SerializeField] private Transform rayOrigin;        // arrastra MainCamera
        [SerializeField] private float range = 2.5f;
        [SerializeField] private LayerMask interactableMask; // solo Layer Interactable
        [SerializeField] private float promptCooldown = 0.2f;

        [Header("Events")]
        [SerializeField] private MonoBehaviour eventBusSource; // arrastra EventBus
        private IEventBus _bus;

        private IInteractable _hover;    // el interactuable actual bajo mira
        private float _nextPromptTime;

        private void Awake()
        {
            _bus = eventBusSource as IEventBus;
            if (_bus == null) Debug.LogError("[PlayerInteractor] eventBusSource no implementa IEventBus");
            if (!rayOrigin) rayOrigin = Camera.main ? Camera.main.transform : null;
        }

        private void Update()
        {
            UpdateHover();
            HandleInput();
        }

        private void UpdateHover()
        {
            _hover = null;

            if (!rayOrigin) return;
            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out var hit, range, interactableMask))
            {
                _hover = hit.collider.GetComponentInParent<IInteractable>();
                if (_hover != null && Time.time >= _nextPromptTime)
                {
                    _nextPromptTime = Time.time + promptCooldown;
                    _bus?.Publish(new ShowPrompt(_hover.Prompt(), 0.5f));
                }
            }
        }

        private void HandleInput()
        {
            if (_hover == null) return;
            if (Input.GetKeyDown(KeyCode.E))  // ← puente rápido; luego lo pasas a tu InputService IMPORTANTE PASAR AL INPUTSERVICE
            {
                _hover.Interact(gameObject);
            }
        }
    }
}
