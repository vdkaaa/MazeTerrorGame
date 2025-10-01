using System.Collections.Generic;
using UnityEngine;

namespace Project.Gameplay.Player
{
    public class PlayerInventory : MonoBehaviour, IInventory
    {
        private readonly Dictionary<string, int> _items = new();


        public bool HasItem(string id) => Count(id) > 0;
        public bool AddItem(string id, int amount = 1)
        {
            if (!_items.ContainsKey(id)) _items[id] = 0;
            _items[id] += amount;
            // TODO: emitir evento InventoryChanged
            return true;
        }

        public bool UseItem(string id)
        {
            if (!_items.TryGetValue(id, out int count) || count <= 0) return false;
            _items[id] = count - 1;
            // TODO: emitir evento InventoryChanged
            return true;
        }


        public List<string> GetAllItems()
        {
            var list = new List<string>();
            foreach (var kvp in _items)
            {
                for (int i = 0; i < kvp.Value; i++)
                    list.Add(kvp.Key);
            }
            return list;
        }

        public void LoadFromList(List<string> items)
        {
            _items.Clear();
            if (items == null) return;
            foreach (var id in items)
                AddItem(id);
        }

        public int Count(string id) => _items.TryGetValue(id, out int c) ? c : 0;
    }
}
