using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISceneLoader
{
    //Es un dato que guarda si queremos que la escena se active
    //automáticamente al terminar de cargar.
    [SerializeField] private bool allowSceneActivation = true;

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //esta vez devuelve un AsyncOperation
    //(un objeto que te deja ver el progreso mientras carga)
    public AsyncOperation LoadAsync(string sceneName)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = allowSceneActivation;
        //devuelve ese objeto para que puedas monitorear
        //op.progress y mostrar una barra de loading
        return op;
    }
}
