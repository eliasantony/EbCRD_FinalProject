using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    public GameObject model1;
    public GameObject model2;
    public GameObject startScreen;
    public GameObject controlsScreen;
    public GameObject gamePrincipleScreen;
    
    public void StartGame()
    {
        SceneManager.LoadScene("ZombieCity");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void ShowControls()
    {
        model1.SetActive(false);
        model2.SetActive(false);
        startScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }
    
    public void ShowGamePrinciple()
    {
        model1.SetActive(false);
        model2.SetActive(false);
        startScreen.SetActive(false);
        gamePrincipleScreen.SetActive(true);
    }
    
    public void BackToStartScreen()
    {
        model1.SetActive(true);
        model2.SetActive(true);
        startScreen.SetActive(true);
        controlsScreen.SetActive(false);
        gamePrincipleScreen.SetActive(false);
    }
}