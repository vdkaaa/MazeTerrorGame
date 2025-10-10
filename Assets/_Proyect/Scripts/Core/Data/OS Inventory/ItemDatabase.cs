using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Data
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Items/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemData> allItems;

        public ItemData GetItem(string itemId)
        {
            return allItems.FirstOrDefault(item => item.itemId == itemId);
        }
    }
}
