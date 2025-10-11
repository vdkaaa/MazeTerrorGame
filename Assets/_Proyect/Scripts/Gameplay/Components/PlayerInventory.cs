using System.Collections.Generic;
using UnityEngine;
using Project.Core.Events.DTOs;
using Project.Data;


namespace Project.Gameplay.Player
{
    public class PlayerInventory : MonoBehaviour, IInventory
    {
        [Header("Data")]
        [SerializeField] private ItemDatabase itemDatabase;
        [Header("Player Config SO")]
        [SerializeField] private PlayerConfig playerConfig;
        [Header("Events")]
        [SerializeField] private MonoBehaviour eventBusSource;
        private IEventBus _bus;

        private void Awake()
        {
            _bus = eventBusSource as IEventBus;
            if (itemDatabase == null)
            {
                Debug.LogError("¡Error Crítico! El ItemDatabase no ha sido asignado en el Inspector.", this.gameObject);
                // Opcional: Desactivar el componente para evitar más errores.
                // this.enabled = false; 
            }
        }

        public bool AddItem(string id, int amount = 1)
        {
            if (itemDatabase.GetItem(id) == null) return false; // No existe el item

            if (!playerConfig.GetItems().ContainsKey(id)) playerConfig.GetItems()[id] = 0;
            playerConfig.GetItems()[id] += amount;
            PublishInventoryChanged();
            return true;
        }

        // ¡Aquí está la magia!
        public bool UseItem(string id)
        {
            // Usamos TryGetValue para ser más eficientes y seguros
            if (!playerConfig.GetItems().TryGetValue(id, out int currentAmount) || currentAmount <= 0)
            {
                return false;
            }

            var itemData = itemDatabase.GetItem(id);
            if (itemData == null) return false;

            // Llama al método Use() del ScriptableObject
            if (itemData.Use(gameObject)) // 'gameObject' es el jugador que usa el item
            {
                playerConfig.GetItems()[id] = currentAmount - 1; // Descuenta el item solo si se usó con éxito
                PublishInventoryChanged();
                return true;
            }

            return false;
        }


        public List<string> GetAllItems()
        {
            var list = new List<string>();
            foreach (var kvp in playerConfig.GetItems())
            {
                for (int i = 0; i < kvp.Value; i++)
                    list.Add(kvp.Key);
            }
            return list;
        }

        public void LoadFromList(List<string> items)
        {
            playerConfig.GetItems().Clear();
            if (items == null) return;
            foreach (var id in items)
                AddItem(id);
            
            // Publicamos el evento una sola vez al final, en lugar de en cada AddItem
            PublishInventoryChanged();
        }

        private void PublishInventoryChanged()
        {
            _bus?.Publish(new InventoryChanged(playerConfig.GetItems()));
        }
    }
}
