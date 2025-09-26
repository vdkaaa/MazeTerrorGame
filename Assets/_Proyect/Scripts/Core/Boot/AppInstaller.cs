using UnityEngine;

// Punto de entrada para inicializar servicios globales del juego
public class AppInstaller : MonoBehaviour
{
    private void Awake()
    {
        // En el futuro aquí se instanciarán/registrarán servicios:
        // - InputService
        // - AudioService
        // - SaveService
        // - EventBus
        // - TimeService
        Debug.Log("[AppInstaller] Bootstrapping global services...");
    }
}
