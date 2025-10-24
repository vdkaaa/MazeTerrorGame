using UnityEngine;
using UnityEngine.SceneManagement;
public class GoToPlayScene : MonoBehaviour
{
    public void GoToPlay()
    {
        SceneManager.LoadScene("Labyrinth_Prototype");
    }
}
