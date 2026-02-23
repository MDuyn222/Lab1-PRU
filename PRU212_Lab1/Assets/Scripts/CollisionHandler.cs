using UnityEngine;

/// <summary>
/// Handles collision detection for the player
/// </summary>
public class CollisionHandler : MonoBehaviour
{
    [Header("Collision Settings")]
    [SerializeField] private int asteroidPenalty = 5;

    /// <summary>
    /// Called when player collides with an object
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Player trigger entered! Collided with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        
        // Check for asteroid collision
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("Asteroid collision detected! Taking damage...");
            HandleAsteroidCollision(collision.gameObject);
        }
    }

    /// <summary>
    /// Handles collision with asteroids
    /// </summary>
    private void HandleAsteroidCollision(GameObject asteroid)
    {
        Debug.Log("HandleAsteroidCollision called");
        
        // Deduct points
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SubtractScore(asteroidPenalty);
            Debug.Log($"Score decreased by {asteroidPenalty}");
        }
        else
        {
            Debug.LogWarning("ScoreManager.Instance is null!");
        }

        // Take damage
        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.TakeDamage(1);
            Debug.Log("Health decreased by 1");
        }
        else
        {
            Debug.LogWarning("HealthManager.Instance is null!");
        }

        // Destroy asteroid
        Destroy(asteroid);
        Debug.Log("Asteroid destroyed");

        // Check if player should be destroyed (handled by HealthManager)
        // Game over will trigger automatically when health reaches 0
    }
}