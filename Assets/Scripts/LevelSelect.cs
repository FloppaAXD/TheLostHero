using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 2 && levelIndex <= 14)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogWarning("Неверный номер уровня: " + levelIndex);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
