using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Gameplay.Player;

namespace Project.Gameplay.Inspect
{
    public class InspectableBox : MonoBehaviour, IInspectable
    {
        [SerializeField] private string prompt = "Point and hold to inspect";
        [SerializeField] private GameObject visuals;
        [SerializeField] private GameObject keyPrefab;          // prefab de llave a spawnear
        [SerializeField] private Transform keySpawnPoint;
        [SerializeField] private MonoBehaviour eventBusSource;  // EventBus
        private IEventBus _bus;
        [SerializeField] private GameObject hoverVisual; // NEW: child used as hover/outline

        private void Awake() { _bus = eventBusSource as IEventBus; if (hoverVisual) hoverVisual.SetActive(false); }

        public string Prompt() => prompt;

        public void Interact(GameObject interactor)
        {
            _bus?.Publish(new ShowPrompt("Something went wrong...", 1f));
            var s = FindFirstObjectByType<Project.Gameplay.Inspect.ScreamerController>();
            s?.TriggerScreamer();
            Destroy(gameObject);
            Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
        }

        public void SetHover(bool on)
        {
            if (hoverVisual) hoverVisual.SetActive(on);
            else
            {
                // fallback: slightly scale visuals as simple feedback
                if (visuals) visuals.transform.localScale = on ? Vector3.one * 1.05f : Vector3.one;
            }
        }

        public void OnExamined(GameObject examiner)
        {
            _bus?.Publish(new ShowPrompt("You found a clue...", 1.5f));
        }

    }
}
