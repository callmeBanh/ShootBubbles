using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        loadingController.LoadScene("GamePlay");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
