using UnityEngine;

namespace Project.Core.Services.Save
{
    public class SaveLoadController : MonoBehaviour
    {
        [SerializeField] private PlayerStateAdapter playerAdapter;
        [SerializeField] private string slotId = "0";
        private PlayerPrefsSaveService _svc;

        private void Awake()
        {
            _svc = new PlayerPrefsSaveService();
            if (!playerAdapter) playerAdapter = FindFirstObjectByType<PlayerStateAdapter>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Guardamos");
                Save();
            }
              

            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("Cargamos");
                Load();
            }
              
        }

        public void Save()
        {
            var state = playerAdapter.Read();
            _svc.SaveGame(state, slotId);
            Debug.Log($"[SaveLoad] Saved slot {slotId}");
        }

        public void Load()
        {
            if (_svc.TryLoadGame(out var state, slotId))
            {
                playerAdapter.Apply(state);
                Debug.Log($"[SaveLoad] Loaded slot {slotId}");
            }
            else
            {
                Debug.LogWarning($"[SaveLoad] No save found for slot {slotId}");
            }
        }

        public void DeleteSlot()
        {
            _svc.DeleteSave(slotId);
            Debug.Log($"[SaveLoad] Deleted slot {slotId}");
        }
    }
}
