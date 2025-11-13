using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("1");
    }

    public void QuitGame()
    {
        Debug.Log("Игра закрывается...");
        Application.Quit();
    }
}
