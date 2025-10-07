using UnityEngine;
using UnityEngine.UI;
using Project.Core.Events.DTOs;
using TMPro;

namespace Project.UI.HUD
{
    public class UISaveSlotIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;                    // Asigna un Text (UI)
        [SerializeField] private string prefix = "Slot: ";      // Texto fijo
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float fadeTime = 1f;
        [SerializeField] private float visibleTime = 1.5f;
        private Coroutine _co;
        private void Awake()
        {
            if (!label) label = GetComponentInChildren<TextMeshProUGUI>();
            if (group) group.alpha = 0f;

        }

        public void SetSlot(string slotId)
        {
            if (label) label.text = prefix + slotId;
            if (group)
            {
                if (_co != null) StopCoroutine(_co);
                _co = StartCoroutine(FadeRoutine());
            }
        }


        private System.Collections.IEnumerator FadeRoutine()
        {
            // in
            float t = 0f; while (t < fadeTime) { t += Time.unscaledDeltaTime; group.alpha = Mathf.Lerp(0, 1, t / fadeTime); yield return null; }
            group.alpha = 1f;
            // hold
            yield return new WaitForSecondsRealtime(visibleTime);
            // out
            t = 0f; while (t < fadeTime) { t += Time.unscaledDeltaTime; group.alpha = Mathf.Lerp(1, 0, t / fadeTime); yield return null; }
            group.alpha = 0f;
        }
    }
}
