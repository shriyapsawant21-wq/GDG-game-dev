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
    
    [Header("hiding the ui for pause and healthbar")]
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject pauseButton; 

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
        HideAllUI();
        startMenu.SetActive(true);
    }

    private void HideAllUI()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        victoryScreen.SetActive(false);
        if (healthBarUI != null) healthBarUI.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(false); 
    }

    public void StartGame()
    {
        gameStarted = true;
        startMenu.SetActive(false);
        
        if (healthBarUI != null) healthBarUI.SetActive(true);
        if (pauseButton != null) pauseButton.SetActive(true); 
        
        Time.timeScale = 1f;

        GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerHealth>()?.UpdateHealthUI();
    }

    public void GameOver() 
    { 
        gameStarted = false;
         gameOverScreen.SetActive(true); 
         healthBarUI.SetActive(false); 
         if(pauseButton) 
         {
            pauseButton.SetActive(false); 
            Time.timeScale = 0f; 
         }
    }
    public void Victory() 
    {
        gameStarted = false; 
        victoryScreen.SetActive(true); 
        healthBarUI.SetActive(false); 
        if(pauseButton) 
        {
            pauseButton.SetActive(false);
            Time.timeScale = 0f; 
        }
    }
    public void ResumeGame() 
    { 
        isPaused = false; 
        pauseMenu.SetActive(false); 
        Time.timeScale = 1f; 
    }
    public void PauseGame() 
    { 
        if (!gameStarted) 
        {
            return;
        } 
        isPaused = true; 
        pauseMenu.SetActive(true); 
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
        Application.Quit();
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
}