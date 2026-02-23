using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles End Game UI interactions
/// </summary>
public class EndGameUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        // Display final score
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {finalScore}";
        }

        // Setup button listeners
        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
    }

    /// <summary>
    /// Called when Play Again button is clicked
    /// </summary>
    private void OnPlayAgainButtonClicked()
    {
        SceneManager.LoadScene("Gameplay");
    }

    /// <summary>
    /// Called when Main Menu button is clicked
    /// </summary>
    private void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Called when Quit button is clicked
    /// </summary>
    private void OnQuitButtonClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}