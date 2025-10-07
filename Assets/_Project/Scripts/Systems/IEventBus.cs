// Contrato para publicar/suscribirse a eventos globales
public interface IEventBus
{
    void Publish<T>(T evt);
    void Subscribe<T>(System.Action<T> handler);
    void Unsubscribe<T>(System.Action<T> handler);
}
