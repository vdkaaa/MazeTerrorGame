using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Gameplay.Player;

namespace Project.Gameplay.Inspect
{
    public class InspectableBox : MonoBehaviour, IInspectable
    {
        [SerializeField] private string prompt = "Point and hold to inspect";
        [SerializeField] private bool isTrap = false;           // si true -> screamer al interactuar sin apuntar
        [SerializeField] private GameObject visuals;
        [SerializeField] private GameObject keyPrefab;          // prefab de llave a spawnear
        [SerializeField] private Transform keySpawnPoint;
        [SerializeField] private MonoBehaviour eventBusSource;  // EventBus
        private IEventBus _bus;
        private bool _revealed = false;

        private void Awake() { _bus = eventBusSource as IEventBus; }

        public string Prompt() => prompt;

        public void Interact(GameObject interactor)
        {
            // Si el jugador presiona E sin haber examinado (raycast no apuntando), se considera mal uso -> screamer
            if (!_revealed && isTrap)
            {
                _bus?.Publish(new ShowPrompt("Something went wrong...", 1f));
                // fire screamer
                var s = FindFirstObjectByType<Project.Gameplay.Inspect.ScreamerController>();
                s?.TriggerScreamer();
                return;
            }

            // si ya revelado, permitir recoger etc. (no implementado aquí)
        }

        public void OnExamined(GameObject examiner)
        {
            if (_revealed) return;
            _revealed = true;
            _bus?.Publish(new ShowPrompt("You found a clue...", 1.5f));
            if (keyPrefab && keySpawnPoint)
            {
                Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
            }
            if (visuals) visuals.SetActive(false); // revela interior si aplica
        }
    }
}
