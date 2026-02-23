using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles Main Menu UI interactions
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject instructionsPanel;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private Button closeInstructionsButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        // Auto-find close button if not assigned
        if (closeInstructionsButton == null && instructionsPanel != null)
        {
            // Find the close button within the instructions panel
            Transform closeButtonTransform = instructionsPanel.transform.Find("CloseButton");
            if (closeButtonTransform != null)
            {
                closeInstructionsButton = closeButtonTransform.GetComponent<Button>();
            }
        }

        // Setup button listeners
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        if (instructionsButton != null)
        {
            instructionsButton.onClick.AddListener(OnInstructionsButtonClicked);
        }

        if (closeInstructionsButton != null)
        {
            closeInstructionsButton.onClick.AddListener(OnCloseInstructionsButtonClicked);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        // Show main panel, hide instructions
        if (mainPanel != null) mainPanel.SetActive(true);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
    }

    /// <summary>
    /// Called when Play button is clicked
    /// </summary>
    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Gameplay");
    }

    /// <summary>
    /// Called when Instructions button is clicked
    /// </summary>
    private void OnInstructionsButtonClicked()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(true);
    }

    /// <summary>
    /// Called when Close Instructions button is clicked
    /// </summary>
    private void OnCloseInstructionsButtonClicked()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
    }

    /// <summary>
    /// Called when Exit button is clicked
    /// </summary>
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}