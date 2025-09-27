using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.HUD
{
    public class UIBattery : MonoBehaviour
    {
        [SerializeField] private Slider slider; // Slider (min 0, max 1)
        public void SetValue(float t) { if (slider) slider.value = Mathf.Clamp01(t); }
    }


}
