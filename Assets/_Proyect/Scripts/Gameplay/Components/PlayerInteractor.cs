using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Data;
using Project.Gameplay.Interaction;
using Project.Gameplay.Inspect; // ...added

namespace Project.Gameplay.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("ConfigRaycast")]
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private Transform rayOrigin;        // arrastra MainCamera

        [Header("Events")]
        [SerializeField] private MonoBehaviour eventBusSource; // arrastra EventBus
        private IEventBus _bus;

        private IInteractable _hover;            // objetivo actual bajo mira
        private IInteractable _lastHover;        // último objetivo al que mostramos prompt
        private string _lastPromptText;          // último texto mostrado
        private float _nextPromptTime;
        private float _suppressUntil;            // no emitir prompts de hover mientras esté activo

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
            var previousHover = _hover; // track previous target
            _hover = null;

            if (!rayOrigin)
            {
                // clear previous hover visual if we lost ray origin
                if (previousHover != null) SetInspectableHover(previousHover, false);
                return;
            }

            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out var hit, playerConfig.GetRange(), playerConfig.GetInteractableMask()))
            {
                _hover = hit.collider.GetComponentInParent<IInteractable>();
            }

            // Update hover visuals when target changed
            if (previousHover != _hover)
            {
                SetInspectableHover(previousHover, false);
                SetInspectableHover(_hover, true);
            }

            // No hover or we’re suppressing hover prompts after an interaction
            if (_hover == null || Time.time < _suppressUntil) return;

            // Only (re)show the prompt if target OR prompt text changed,
            // and we respect a small cooldown to avoid spam.
            if (Time.time >= _nextPromptTime)
            {
                string promptText = _hover.Prompt();
                bool targetChanged = _hover != _lastHover;
                bool textChanged = promptText != _lastPromptText;

                if (targetChanged || textChanged)
                {
                    _nextPromptTime = Time.time + playerConfig.GetPromptCooldown();
                    _lastHover = _hover;
                    _lastPromptText = promptText;
                    _bus?.Publish(new ShowPrompt(promptText, 1f));
                }
            }
        }

        // New helper to toggle hover visuals on InspectableBox instances
        private void SetInspectableHover(IInteractable interactable, bool on)
        {
            if (interactable == null) return;

            // If the underlying Unity object was destroyed, skip safely
            if (interactable is UnityEngine.Object uo && uo == null) return;

            // Direct cast if the interactable is the concrete InspectableBox
            if (interactable is InspectableBox directBox)
            {
                if (directBox != null) directBox.SetHover(on);
                return;
            }

            // Otherwise try to resolve from a MonoBehaviour and find the InspectableBox in parents
            if (interactable is MonoBehaviour mb)
            {
                if (mb == null) return; // destroyed check
                var box = mb.GetComponentInParent<InspectableBox>();
                if (box != null)
                {
                    box.SetHover(on);
                }
                // silent fallback: don't warn every time (avoids spam for other IInteractable types)
                return;
            }

            // Other IInteractable implementations: nothing to do
        }

        private void HandleInput()
        {
            if (_hover == null) return;

            if (Input.GetKeyDown(KeyCode.E))
            {

                // Call the interaction. It may publish its own prompt (e.g., 1.5f “locked”).
                _hover.Interact(gameObject);

                // Suppress hover prompts long enough so the interaction message isn't overwritten.
                // You can fine-tune this; 1.6f matches your 1.5s message.
                _suppressUntil = Time.time + 1.6f;

                // Force a refresh after suppression ends so hover prompt shows again if still looking
                _lastPromptText = null;
            }
        }
    }
}
