using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.HUD
{
    public class UIHealth : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        public void SetValue(float t) { if (slider) slider.value = Mathf.Clamp01(t); }
    }
}
