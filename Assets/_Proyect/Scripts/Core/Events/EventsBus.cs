using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour, IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _listeners = new();

    public void Publish<T>(T evt)
    {
        var type = typeof(T);
        if (!_listeners.TryGetValue(type, out var list)) return;
        // Copia local para evitar modificación durante iteración
        var snapshot = list.ToArray();
        foreach (var d in snapshot)
            (d as Action<T>)?.Invoke(evt);
    }

    public void Subscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (!_listeners.TryGetValue(type, out var list))
        {
            list = new List<Delegate>();
            _listeners[type] = list;
        }
        if (!list.Contains(handler)) list.Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (_listeners.TryGetValue(type, out var list))
        {
            list.Remove(handler);
            if (list.Count == 0) _listeners.Remove(type);
        }
    }
}
