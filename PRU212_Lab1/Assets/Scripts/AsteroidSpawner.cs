using UnityEngine;

/// <summary>
/// Spawns asteroids at random positions
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private int initialAsteroidCount = 5;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxAsteroids = 10;

    [Header("Difficulty Settings")]
    [SerializeField] private int scorePerDifficultyLevel = 50;
    [SerializeField] private float spawnIntervalDecrease = 0.5f;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private int maxAsteroidsIncrease = 2;
    [SerializeField] private int absoluteMaxAsteroids = 25;
    [SerializeField] private float speedIncreasePerLevel = 0.2f;
    [SerializeField] private float maxSpeedMultiplier = 3f;

    [Header("Spawn Area")]
    [SerializeField] private float spawnRangeX = 8f;
    [SerializeField] private float spawnRangeY = 4.5f;

    private float nextSpawnTime;
    private int currentAsteroidCount;
    private int currentDifficultyLevel = 0;

private void Start()
    {
        // Spawn initial asteroids
        for (int i = 0; i < initialAsteroidCount; i++)
        {
            SpawnAsteroid();
        }
        nextSpawnTime = Time.time + GetCurrentSpawnInterval();
    }

private void Update()
    {
        // Update difficulty based on score
        UpdateDifficulty();

        // Periodically spawn new asteroids
        if (Time.time >= nextSpawnTime && currentAsteroidCount < GetCurrentMaxAsteroids())
        {
            SpawnAsteroid();
            nextSpawnTime = Time.time + GetCurrentSpawnInterval();
        }
    }

    /// <summary>
    /// Spawns a single asteroid at a random position
    /// </summary>
private void SpawnAsteroid()
    {
        if (asteroidPrefab == null)
        {
            Debug.LogWarning("Asteroid prefab not assigned!");
            return;
        }

        Vector2 spawnPosition = new Vector2(
            UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
            UnityEngine.Random.Range(-spawnRangeY, spawnRangeY)
        );

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        currentAsteroidCount++;

        // Apply speed multiplier based on difficulty
        Asteroid asteroidComponent = asteroid.GetComponent<Asteroid>();
        if (asteroidComponent != null)
        {
            asteroidComponent.SetSpeedMultiplier(GetSpeedMultiplier());
            asteroidComponent.OnDestroyed += () => currentAsteroidCount--;
        }
    }


/// <summary>
    /// Updates difficulty based on current score
    /// </summary>
    private void UpdateDifficulty()
    {
        if (ScoreManager.Instance == null) return;

        int currentScore = ScoreManager.Instance.CurrentScore;
        int newLevel = CalculateDifficultyLevel(currentScore);

        if (newLevel != currentDifficultyLevel)
        {
            currentDifficultyLevel = newLevel;
            Debug.Log($"Difficulty increased to level {currentDifficultyLevel}!");
        }
    }

    /// <summary>
    /// Calculates difficulty level based on score
    /// </summary>
    private int CalculateDifficultyLevel(int score)
    {
        return Mathf.FloorToInt(score / scorePerDifficultyLevel);
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
    /// Gets current max asteroids based on difficulty
    /// </summary>
    private int GetCurrentMaxAsteroids()
    {
        int max = maxAsteroids + (currentDifficultyLevel * maxAsteroidsIncrease);
        return Mathf.Min(max, absoluteMaxAsteroids);
    }

    /// <summary>
    /// Gets current asteroid speed multiplier based on difficulty
    /// </summary>
    private float GetSpeedMultiplier()
    {
        float multiplier = 1f + (currentDifficultyLevel * speedIncreasePerLevel);
        return Mathf.Min(multiplier, maxSpeedMultiplier);
    }
}
