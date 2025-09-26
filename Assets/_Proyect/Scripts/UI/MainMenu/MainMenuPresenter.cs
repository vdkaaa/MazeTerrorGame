using UnityEngine;

public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] private MainMenuView view;
    [SerializeField] private SceneLoader sceneLoader; // depende de la interfaz a nivel concepto

    private ISceneLoader _loader;

    private void Awake()
    {
        // Inversión de dependencias a nivel runtime (simple):
        _loader = sceneLoader as ISceneLoader;
        if (_loader == null)
            Debug.LogError("[MainMenuPresenter] SceneLoader no asignado o no implementa ISceneLoader.");
    }

    private void OnEnable()
    {
        if (view == null) return;
        view.PlayClicked += HandlePlay;
        view.QuitClicked += HandleQuit;
    }

    private void OnDisable()
    {
        if (view == null) return;
        view.PlayClicked -= HandlePlay;
        view.QuitClicked -= HandleQuit;
    }

    private void HandlePlay()
    {
        // Único lugar donde se decide qué escena cargar:
        _loader?.Load(SceneNames.LabyrinthPrototype);
    }

    private void HandleQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
