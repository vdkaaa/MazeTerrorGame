using UnityEngine;

// Punto de entrada para inicializar servicios globales del juego
public class AppInstaller : MonoBehaviour
{
    private void Awake()
    {
        // En el futuro aqu� se instanciar�n/registrar�n servicios:
        // - InputService
        // - AudioService
        // - SaveService
        // - EventBus
        // - TimeService
        Debug.Log("[AppInstaller] Bootstrapping global services...");
    }
}
