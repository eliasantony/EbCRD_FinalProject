using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("ZombieCity");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}