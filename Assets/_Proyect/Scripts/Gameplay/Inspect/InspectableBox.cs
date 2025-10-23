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


        private void Awake() { _bus = eventBusSource as IEventBus; }

        public string Prompt() => prompt;

        public void Interact(GameObject interactor)
        {
            // Si el jugador presiona E 
            _bus?.Publish(new ShowPrompt("Something went wrong...", 1f));
            // fire screamer
            var s = FindFirstObjectByType<Project.Gameplay.Inspect.ScreamerController>();
            s?.TriggerScreamer();
            Destroy(gameObject);
            Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
            // si ya revelado, permitir recoger etc. (no implementado aqu�)
        }

        public void OnExamined(GameObject examiner)
        {
            _bus?.Publish(new ShowPrompt("You found a clue...", 1.5f));
        }

    }
}
