using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    public void HideHUD()
    {
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public void ShowHUD()
    {
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }
}
