using UnityEngine;
using TMPro;

/// <summary>
/// Manages player score
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int startingScore = 0;

    private int currentScore;

    public int CurrentScore => currentScore;

    private void Awake()
    {
        // Singleton pattern (scene-specific, not persistent)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        // Clean up singleton reference
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        currentScore = startingScore;
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Adds points to the score
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Subtracts points from the score
    /// </summary>
    public void SubtractScore(int points)
    {
        currentScore = Mathf.Max(0, currentScore - points);
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Resets the score to zero
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    /// <summary>
    /// Updates the score display UI
    /// </summary>
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }
}