using UnityEngine;

/// <summary>
/// Spawns collectible stars
/// </summary>
public class StarSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private int initialStarCount = 8;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxStars = 15;

    [Header("Difficulty Settings")]
    [SerializeField] private int scorePerDifficultyLevel = 50;
    [SerializeField] private float spawnIntervalDecrease = 0.3f;
    [SerializeField] private float minSpawnInterval = 1.5f;
    [SerializeField] private int maxStarsIncrease = 3;
    [SerializeField] private int absoluteMaxStars = 30;

    [Header("Spawn Area")]
    [SerializeField] private float spawnRangeX = 8f;
    [SerializeField] private float spawnRangeY = 4.5f;

    private float nextSpawnTime;
    private int currentStarCount;
    private int currentDifficultyLevel = 0;

private void Start()
    {
        // Spawn initial stars
        for (int i = 0; i < initialStarCount; i++)
        {
            SpawnStar();
        }
        nextSpawnTime = Time.time + GetCurrentSpawnInterval();
    }

private void Update()
    {
        // Update difficulty based on score
        UpdateDifficulty();

        // Periodically spawn new stars
        if (Time.time >= nextSpawnTime && currentStarCount < GetCurrentMaxStars())
        {
            SpawnStar();
            nextSpawnTime = Time.time + GetCurrentSpawnInterval();
        }
    }

    /// <summary>
    /// Spawns a single star at a random position
    /// </summary>
    private void SpawnStar()
    {
        if (starPrefab == null)
        {
            Debug.LogWarning("Star prefab not assigned!");
            return;
        }

        Vector2 spawnPosition = new Vector2(
            UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
            UnityEngine.Random.Range(-spawnRangeY, spawnRangeY)
        );

        GameObject star = Instantiate(starPrefab, spawnPosition, Quaternion.identity);
        currentStarCount++;

        // Subscribe to collection event
        Star starComponent = star.GetComponent<Star>();
        if (starComponent != null)
        {
            starComponent.OnCollected += () => currentStarCount--;
        }
    }


/// <summary>
    /// Updates difficulty based on current score
    /// </summary>
    private void UpdateDifficulty()
    {
        if (ScoreManager.Instance == null) return;

        int currentScore = ScoreManager.Instance.CurrentScore;
        int newLevel = Mathf.FloorToInt(currentScore / scorePerDifficultyLevel);

        if (newLevel != currentDifficultyLevel)
        {
            currentDifficultyLevel = newLevel;
        }
    }

    /// <summary>
    /// Gets current spawn interval based on difficulty
    /// </summary>
    private float GetCurrentSpawnInterval()
    {
        float interval = spawnInterval - (currentDifficultyLevel * spawnIntervalDecrease);
        return Mathf.Max(interval, minSpawnInterval);
    }

    /// <summary>
    /// Gets current max stars based on difficulty
    /// </summary>
    private int GetCurrentMaxStars()
    {
        int max = maxStars + (currentDifficultyLevel * maxStarsIncrease);
        return Mathf.Min(max, absoluteMaxStars);
    }
}