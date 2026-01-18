using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject victoryScreen;

    private bool isPaused = false;
    private bool gameStarted = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 0f;
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }

    void Update()
    {
        if (gameStarted && !gameOverScreen.activeSelf && !victoryScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        startMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Victory()
    {
        victoryScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}