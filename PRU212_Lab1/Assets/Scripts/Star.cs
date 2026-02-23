using UnityEngine;
using System;

/// <summary>
/// Collectible star behavior
/// </summary>
public class Star : MonoBehaviour
{
    [Header("Star Settings")]
    [SerializeField] private int pointValue = 10;
    [SerializeField] private float rotationSpeed = 100f;

    public event Action OnCollected;

    private void Update()
    {
        // Rotate the star for visual effect
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Called when player collects the star
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Add score
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(pointValue);
            }

            // Notify power-up manager
            if (PowerUpManager.Instance != null)
            {
                PowerUpManager.Instance.OnStarCollected();
            }

            // Notify spawner
            OnCollected?.Invoke();

            // Destroy star
            Destroy(gameObject);
        }
    }
}