using System;
using UnityEngine;

// Servicio central para eventos del juego (stub inicial)
public class EventBus : MonoBehaviour, IEventBus
{
    public void Publish<T>(T evt)
    {
        // TODO: implementar lógica de publicación
        Debug.Log($"[EventBus] Publish {typeof(T).Name}");
    }

    public void Subscribe<T>(Action<T> handler)
    {
        // TODO: implementar suscripción
        Debug.Log($"[EventBus] Subscribe {typeof(T).Name}");
    }

    public void Unsubscribe<T>(Action<T> handler)
    {
        // TODO: implementar desuscripción
        Debug.Log($"[EventBus] Unsubscribe {typeof(T).Name}");
    }
}
