using UnityEngine;

namespace Project.Core.Services.Save
{
    public class PlayerPrefsSaveService : ISaveService
    {
        private const string KEY = "MazeTerror_SaveSlot_0";

        public void SaveGame(string slotId)
        {
            Debug.LogWarning("[PlayerPrefsSaveService] Use SaveGame(GameState, slotId) overload.");
        }

        public void LoadGame(string slotId)
        {
            Debug.LogWarning("[PlayerPrefsSaveService] Use LoadGame(slotId) with return GameState overload.");
        }

        public void DeleteSave(string slotId)
        {
            PlayerPrefs.DeleteKey(key(slotId));
            PlayerPrefs.Save();
        }

        // Overloads prácticos:
        public void SaveGame(GameState state, string slotId = "0")
        {
            var json = JsonUtility.ToJson(state);
            PlayerPrefs.SetString(key(slotId), json);
            PlayerPrefs.Save();
        }

        public bool TryLoadGame(out GameState state, string slotId = "0")
        {
            var k = key(slotId);
            if (!PlayerPrefs.HasKey(k))
            {
                state = default;
                return false;
            }
            var json = PlayerPrefs.GetString(k);
            state = JsonUtility.FromJson<GameState>(json);
            return true;
        }

        private string key(string slotId) => $"MazeTerror_SaveSlot_{slotId}";
    }
}
