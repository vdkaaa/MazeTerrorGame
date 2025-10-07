public interface ISceneLoader 
{
    void Load(string sceneName);
    UnityEngine.AsyncOperation LoadAsync (string sceneName);
}
