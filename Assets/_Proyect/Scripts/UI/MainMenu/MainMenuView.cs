using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton; //(para editor/standalone)

    public event Action PlayClicked;
    public event Action QuitClicked;

    private void OnEnable()
    {
        if (playButton) playButton.onClick.AddListener(OnPlay);
        if (quitButton) quitButton.onClick.AddListener(OnQuit);
    }

    private void OnDisable()
    {
        if (playButton) playButton.onClick.RemoveListener(OnPlay);
        if (quitButton) quitButton.onClick.RemoveListener(OnQuit);
    }

    private void OnPlay() => PlayClicked?.Invoke();
    private void OnQuit() => QuitClicked?.Invoke();
}
