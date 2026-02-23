using UnityEngine;

/// <summary>
/// Manages player power-ups from collecting stars
/// </summary>
public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    [Header("Speed Boost Settings")]
    [SerializeField] private float speedIncreasePerStar = 0.1f;
    [SerializeField] private float maxSpeedBonus = 3f;

    [Header("Fire Rate Boost Settings")]
    [SerializeField] private float fireRateDecreasePerStar = 0.008f;
    [SerializeField] private float maxFireRateDecrease = 0.15f;

    private int starsCollected = 0;
    private float currentSpeedBonus = 0f;
    private float currentFireRateBonus = 0f;

    public float CurrentSpeedBonus => currentSpeedBonus;
    public float CurrentFireRateBonus => currentFireRateBonus;
    public int StarsCollected => starsCollected;

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

    /// <summary>
    /// Called when player collects a star
    /// </summary>
    public void OnStarCollected()
    {
        starsCollected++;

        // Calculate speed bonus (capped)
        currentSpeedBonus = Mathf.Min(starsCollected * speedIncreasePerStar, maxSpeedBonus);

        // Calculate fire rate bonus (capped) - negative because it reduces cooldown
        currentFireRateBonus = Mathf.Min(starsCollected * fireRateDecreasePerStar, maxFireRateDecrease);

        Debug.Log($"Star collected! Total: {starsCollected} | Speed Bonus: +{currentSpeedBonus:F2} | Fire Rate Bonus: -{currentFireRateBonus:F3}s");
    }

    /// <summary>
    /// Resets all power-ups (called when game restarts)
    /// </summary>
    public void ResetPowerUps()
    {
        starsCollected = 0;
        currentSpeedBonus = 0f;
        currentFireRateBonus = 0f;
        Debug.Log("Power-ups reset!");
    }
}

