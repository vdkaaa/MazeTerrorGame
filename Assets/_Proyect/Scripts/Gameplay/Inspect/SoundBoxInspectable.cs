using UnityEngine;
using Project.Core.Events.DTOs;
using Unity.VisualScripting;
using Project.Data;

namespace Project.Gameplay.Inspect
{
    public class SoundBoxInspectable : MonoBehaviour, IInspectable
    {
        [SerializeField] private string prompt = "Press E for Inspect";
        [SerializeField] private MonoBehaviour eventBusSource;  // EventBus
        private IEventBus _bus;
        private void Awake() { _bus = eventBusSource as IEventBus; }
        public string Prompt() => prompt;

        [Header("Contador del Puzzle")]
        [SerializeField] private CounterSO soundboxCounter;



        public void OnExamined(GameObject interactor)
        {
             _bus?.Publish(new ShowPrompt("It emits a strage hum...", 3f));
        }

        public void Interact(GameObject interactor)
        {
            _bus?.Publish(new ShowPrompt("The hum intesifies briefly", 3f));
            soundboxCounter.Increment();
            Destroy(gameObject);
        }
    }
}