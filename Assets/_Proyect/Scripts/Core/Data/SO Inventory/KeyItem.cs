// En una nueva carpeta, por ejemplo: Scripts/Gameplay/Items
using UnityEngine;
using Project.Core.Events.DTOs;

namespace Project.Data
{  
    [CreateAssetMenu(fileName = "NewKeyItem", menuName = "Items/Key Item")]
    public class KeyItem : ItemData
    {
        /// <summary>
        /// Define qué sucede cuando el jugador intenta "usar" la llave desde el inventario.
        /// </summary>
        /// <param name="user">El GameObject del jugador que usa el item.</param>
        /// <returns>
        /// Devuelve 'false' porque una llave no se consume. Su propósito es ser verificada
        /// por otros objetos (como puertas) que miran en el inventario del jugador.
        /// </returns>
        public override bool Use(GameObject user)
        {
            // Opcional: Podemos mostrar un mensaje al jugador si intenta usar la llave.
            var bus = Object.FindFirstObjectByType<EventBus>() as IEventBus;
            bus?.Publish(new ShowPrompt("This key might open a specific door.", 1.5f));
            // Devuelve 'false' para que el objeto NO se consuma del inventario.
            return false;
        }
    }
}