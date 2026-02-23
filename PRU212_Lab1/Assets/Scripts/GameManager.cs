using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages overall game state
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    private bool isGameOver = false;

    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Subscribe to scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene loaded event
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    /// <summary>
    /// Called when a new scene is loaded
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset game over state when Gameplay scene loads
        if (scene.name == "Gameplay")
        {
            // Cancel any pending Invoke calls (like LoadEndGameScene)
            CancelInvoke();
            
            isGameOver = false;
            Debug.Log("Gameplay scene loaded, isGameOver reset to false");
            
            // Reset power-ups when starting new game
            // Use invoke to give PowerUpManager time to initialize
            Invoke(nameof(ResetPowerUps), 0.1f);
        }
    }

    /// <summary>
    /// Resets power-ups at the start of a new game
    /// </summary>
    private void ResetPowerUps()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.ResetPowerUps();
        }
    }

    /// <summary>
    /// Triggers game over state
    /// </summary>
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Game Over!");
            
            // Immediately disable player controls
            DisablePlayerControls();
            
            // Save final score
            if (ScoreManager.Instance != null)
            {
                PlayerPrefs.SetInt("FinalScore", ScoreManager.Instance.CurrentScore);
                PlayerPrefs.Save();
            }

            // Load end game scene after a short delay
            Invoke(nameof(LoadEndGameScene), 1.5f);
        }
    }

    /// <summary>
    /// Disables player controls to prevent further gameplay
    /// </summary>
    private void DisablePlayerControls()
    {
        // Find and disable player GameObject
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Disable player movement and shooting scripts
            PlayerController controller = player.GetComponent<PlayerController>();
            if (controller != null) controller.enabled = false;
            
            PlayerShooting shooting = player.GetComponent<PlayerShooting>();
            if (shooting != null) shooting.enabled = false;
            
            CollisionHandler collisionHandler = player.GetComponent<CollisionHandler>();
            if (collisionHandler != null) collisionHandler.enabled = false;
            
            Debug.Log("Player controls disabled");
        }
    }

    /// <summary>
    /// Loads the end game scene
    /// </summary>
    private void LoadEndGameScene()
    {
        SceneManager.LoadScene("EndGame");
    }

    /// <summary>
    /// Starts a new game
    /// </summary>
    public void StartNewGame()
    {
        // Reset game over state
        isGameOver = false;
        Debug.Log("Starting new game, isGameOver reset to false");
        
        SceneManager.LoadScene("Gameplay");
    }

    /// <summary>
    /// Returns to main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        // Reset game over state
        isGameOver = false;
        Debug.Log("Returning to main menu, isGameOver reset to false");
        
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}