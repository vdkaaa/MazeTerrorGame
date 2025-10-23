using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_GoToMenu : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
