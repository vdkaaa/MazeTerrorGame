using UnityEngine;

public class BootStartup : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private bool loadAsync = false; // Si saco la pantalla de carga
    [SerializeField] private float delaySeconds = 0.05f;

    private ISceneLoader _loader;

    private void Awake()
    {
        // DIP: trabajamos contra ISceneLoader, no contra SceneManager directamente
        _loader = sceneLoader as ISceneLoader;
        if (_loader == null)
        {
            Debug.LogError("[BootStartup] SceneLoader no asignado o no implementa ISceneLoader.");
        }
    }

    private void Start()
    {
        if (_loader == null) return;
        if (loadAsync)
            StartCoroutine(LoadNextAsync());
        else
            LoadNext();
    }

    private void LoadNext()
    {
        _loader.Load(SceneNames.MainMenu);

    }

    private System.Collections.IEnumerator LoadNextAsync()
    {
        // pequeño delay para permitir que AppInstaller/servicios arranquen si los necesitas
        yield return new WaitForSeconds(delaySeconds);

        var op = _loader.LoadAsync(SceneNames.MainMenu);
        // si más adelante usas pantalla de loading, aquí puedes esperar el progreso.
        // while (!op.isDone) yield return null;
    }


}
