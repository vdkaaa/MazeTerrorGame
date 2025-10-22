using UnityEngine;
using Project.Data; // Para CounterSO
using Project.Core.Events.DTOs; // Para ShowPrompt

namespace Project.Gameplay.Puzzles
{
    public class CounterPuzzleController : MonoBehaviour
    {
        [Header("Configuración del Puzzle")]
        [SerializeField] private CounterSO counterToWatch; // Arrastra aquí tu "SoundBoxCounter" SO
        [SerializeField] private bool triggerOnce = true;

        [Header("Recompensa")]
        [SerializeField] private GameObject keyPrefab;
        [SerializeField] private Transform keySpawnPoint;
        [SerializeField] private string successMessage = "A faint click echoes nearby...";

        [Header("Events")]
        [SerializeField] private MonoBehaviour eventBusSource;
        private IEventBus _bus;

        private bool _isTriggered = false;

        private void Awake()
        {
            _bus = eventBusSource as IEventBus;
        }

        private void OnEnable()
        {
            if (counterToWatch != null)
            {
                // Nos suscribimos al evento del contador
                counterToWatch.OnTargetReached += HandleTargetReached;
            }
        }

        private void OnDisable()
        {
            if (counterToWatch != null)
            {
                // Es MUY importante desuscribirse para evitar errores
                counterToWatch.OnTargetReached -= HandleTargetReached;
            }
        }

        /// <summary>
        /// Este es el "Caso de Uso". Se ejecuta cuando el contador alcanza su objetivo.
        /// </summary>
        private void HandleTargetReached()
        {
            if (triggerOnce && _isTriggered) return;
            _isTriggered = true;

            Debug.Log("¡El puzzle del contador se ha completado!");

            // Lógica de la recompensa
            if (keyPrefab && keySpawnPoint)
            {
                Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
                _bus?.Publish(new ShowPrompt(successMessage, 2f));
            }
            else
            {
                Debug.LogWarning("El puzzle del contador se completó, pero no hay recompensa configurada.");
            }
        }
    }
}
