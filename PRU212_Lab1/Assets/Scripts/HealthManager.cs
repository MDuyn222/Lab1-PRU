using UnityEngine;
using TMPro;

/// <summary>
/// Manages player health
/// </summary>
public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI healthText;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        // Singleton pattern
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

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
        Debug.Log($"HealthManager started: Health = {currentHealth}/{maxHealth}");
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
    /// Reduces health by specified amount
    /// </summary>
    public void TakeDamage(int damage)
    {
        // Don't take damage if already dead
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateHealthDisplay();

        Debug.Log($"Health now: {currentHealth}/{maxHealth}");

        // Check for game over
        if (currentHealth <= 0)
        {
            Debug.Log("Health reached 0! Triggering game over...");
            TriggerGameOver();
        }
    }

    /// <summary>
    /// Heals the player by specified amount
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthDisplay();
    }

    /// <summary>
    /// Resets health to maximum
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    /// <summary>
    /// Updates the health display UI
    /// </summary>
    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
        }
    }

    /// <summary>
    /// Triggers game over
    /// </summary>
    private void TriggerGameOver()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}