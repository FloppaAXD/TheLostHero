using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused = false;
    private PlayerMovement player; // кэшируем ссылку

    private void Start()
    {
        // безопасный поиск игрока
        player = FindFirstObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            // ESC на сцене Level Select = возврат в главное меню
            if (sceneIndex == 1)
            {
                SceneManager.LoadScene(0);
                return;
            }

            // ESC на уровнях игры
            if (sceneIndex >= 2 && sceneIndex <= 9)
            {
                if (!isPaused) Pause();
                else Resume();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        if (player != null)
            player.CanMove = false;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (player != null)
            player.CanMove = true;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
