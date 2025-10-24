// path: Assets/_Proyect/Scripts/Core/Events/DTOs/PlayerTookDamage.cs
namespace Project.Core.Events.DTOs
{
    /// <summary>
    /// Evento que se publica cuando el jugador recibe daño.
    /// </summary>
    public readonly struct PlayerTookDamage
    {
        // De momento no necesita datos, pero podríamos añadir la cantidad
        // de daño si quisiéramos que el sonido varíe en intensidad.
        // public readonly float DamageAmount;
        // public PlayerTookDamage(float damageAmount) => DamageAmount = damageAmount;
    }
}
