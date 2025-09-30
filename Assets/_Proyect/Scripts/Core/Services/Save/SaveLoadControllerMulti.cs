using UnityEngine;
using Project.Core.Events.DTOs;

namespace Project.Core.Services.Save
{
    [DisallowMultipleComponent]
    public class SaveLoadControllerMulti : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private PlayerStateAdapter playerAdapter;
        [SerializeField] private string[] slots = new[] { "1", "2", "3" }; // F1, F2, F3
        [SerializeField] private KeyCode saveKey = KeyCode.F5;
        [SerializeField] private KeyCode loadKey = KeyCode.F9;
        [SerializeField] private float keyCooldown = 0.25f;

        [Header("Events")]
        [SerializeField] private MonoBehaviour eventBusSource; // arrastra EventBus
        private IEventBus _bus;

        private PlayerPrefsSaveService _svc;
        private float _cooldown;
        private int _activeIdx;
        private const string LAST_SLOT_KEY = "MazeTerror_LastSlot";

        private void Awake()
        {
            _svc = new PlayerPrefsSaveService();
            if (!playerAdapter) playerAdapter = FindFirstObjectByType<PlayerStateAdapter>();
            _bus = eventBusSource as IEventBus;

            // Carga el último slot usado (si existe)
            _activeIdx = Mathf.Clamp(PlayerPrefs.GetInt(LAST_SLOT_KEY, 0), 0, slots.Length - 1);
            Announce($"Active slot: {slots[_activeIdx]} (F1/F2/F3)");
        }

        private void Update()
        {
            if (_cooldown > 0f) { _cooldown -= Time.unscaledDeltaTime; return; }

            // Selección de slot (F1/F2/F3)
            if (Input.GetKeyDown(KeyCode.F1)) SetActiveSlot(0);
            if (Input.GetKeyDown(KeyCode.F2)) SetActiveSlot(1);
            if (Input.GetKeyDown(KeyCode.F3)) SetActiveSlot(2);

            // Guardar / Cargar
            if (Input.GetKeyDown(saveKey)) { Save(); _cooldown = keyCooldown; }
            if (Input.GetKeyDown(loadKey)) { Load(); _cooldown = keyCooldown; }
        }

        private void SetActiveSlot(int idx)
        {
            if (idx < 0 || idx >= slots.Length) return;
            _activeIdx = idx;
            PlayerPrefs.SetInt(LAST_SLOT_KEY, _activeIdx);
            PlayerPrefs.Save();
            Announce($"Active slot: {slots[_activeIdx]}");
        }

        private string ActiveSlotId() => slots[Mathf.Clamp(_activeIdx, 0, slots.Length - 1)];

        public void Save()
        {
            var state = playerAdapter.Read();
            _svc.SaveGame(state, ActiveSlotId());
            Announce($"Saved slot {ActiveSlotId()}");
        }

        public void Load()
        {
            if (_svc.TryLoadGame(out var state, ActiveSlotId()))
            {
                playerAdapter.Apply(state);
                Announce($"Loaded slot {ActiveSlotId()}");
            }
            else
            {
                Announce($"No save in slot {ActiveSlotId()}");
            }
        }

        public void DeleteActiveSlot()
        {
            _svc.DeleteSave(ActiveSlotId());
            Announce($"Deleted slot {ActiveSlotId()}");
        }

        private void Announce(string msg) => _bus?.Publish(new ShowPrompt(msg, 1.2f));
    }
}
