using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.HUD
{
    public class UIPrompt : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private CanvasGroup group; // opcional para fade
        private Coroutine _hideRoutine;

        private void Awake()
        {
            if (group) { group.alpha = 0f; group.interactable = false; group.blocksRaycasts = false; }
            if (promptText) promptText.text = string.Empty;
        }

        public void Show(string message, float duration = 0f)
        {
            if (promptText) promptText.text = message;
            if (group) group.alpha = 1f;

            if (_hideRoutine != null) StopCoroutine(_hideRoutine);
            if (duration > 0f)
                _hideRoutine = StartCoroutine(HideAfter(duration));
        }

        public void Hide()
        {
            if (_hideRoutine != null) StopCoroutine(_hideRoutine);
            if (group) group.alpha = 0f;
            if (promptText) promptText.text = string.Empty;
        }

        private IEnumerator HideAfter(float t)
        {
            yield return new WaitForSeconds(t);
            Hide();
        }
    }
}
