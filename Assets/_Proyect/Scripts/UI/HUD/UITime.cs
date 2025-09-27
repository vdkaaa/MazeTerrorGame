using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.HUD
{
    public class UITime : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI clockText; // "mm:ss"
        public void SetClock(int minutes, int seconds)
        {
            if (!clockText) return;
            minutes = Mathf.Max(0, minutes);
            seconds = Mathf.Clamp(seconds, 0, 59);
            clockText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
