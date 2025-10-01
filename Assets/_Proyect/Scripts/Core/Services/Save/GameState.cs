using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Services.Save
{
    [System.Serializable]
    public struct GameState
    {
        public float playerHealth;
        public float playerMaxHealth;
        public float flashlightBattery01; // 0..1
        public Vector3 playerPosition;
        public Vector3 playerForward;     // orientación simple
        public List<string> inventoryItems;

    }
}
