// Contrato para exponer tiempo global del juego
public interface ITimeService
{
    float DeltaTime { get; }
    float TimeSinceStart { get; }
}
