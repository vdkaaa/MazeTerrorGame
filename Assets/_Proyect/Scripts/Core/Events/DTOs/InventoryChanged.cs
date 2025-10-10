using System.Collections.Generic;

namespace Project.Core.Events.DTOs
{
    // Este struct contendrá el estado actual del inventario.
    public struct InventoryChanged
    {
        public readonly Dictionary<string, int> Items;

        public InventoryChanged(Dictionary<string, int> items)
        {
            // Hacemos una copia para evitar que se modifique el diccionario original por referencia.
            Items = new Dictionary<string, int>(items);
        }
    }
}
