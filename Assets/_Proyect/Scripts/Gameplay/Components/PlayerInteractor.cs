using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Data;
using Project.Gameplay.Interaction;

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
            _hover = null;

            if (!rayOrigin) return;

            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out var hit, playerConfig.GetRange(), playerConfig.GetInteractableMask()))
            {
                _hover = hit.collider.GetComponentInParent<IInteractable>();
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
