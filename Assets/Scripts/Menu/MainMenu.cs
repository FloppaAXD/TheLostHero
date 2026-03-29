using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SkinMenu()
    {
        SceneManager.LoadScene(16);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}