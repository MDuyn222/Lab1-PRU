using UnityEngine;
using System;

/// <summary>
/// Controls asteroid movement and behavior
/// </summary>
public class Asteroid : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Boundaries")]
    [SerializeField] private float boundaryX = 10f;
    [SerializeField] private float boundaryY = 6f;

    [Header("Score Settings")]
    [SerializeField] private int destroyBonus = 15;

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private float speedMultiplier = 1f;

    public event Action OnDestroyed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }

        // Set random movement direction
        float angle = UnityEngine.Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }

/// <summary>
    /// Sets the speed multiplier for this asteroid
    /// </summary>
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }


private void Update()
    {
        // Move asteroid with speed multiplier
        rb.linearVelocity = moveDirection * moveSpeed * speedMultiplier;

        // Rotate asteroid
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Wrap around screen edges
        WrapPosition();
    }

    /// <summary>
    /// Wraps asteroid position when it goes off screen
    /// </summary>
    private void WrapPosition()
    {
        Vector3 pos = transform.position;
        bool wrapped = false;

        if (pos.x > boundaryX)
        {
            pos.x = -boundaryX;
            wrapped = true;
        }
        else if (pos.x < -boundaryX)
        {
            pos.x = boundaryX;
            wrapped = true;
        }

        if (pos.y > boundaryY)
        {
            pos.y = -boundaryY;
            wrapped = true;
        }
        else if (pos.y < -boundaryY)
        {
            pos.y = boundaryY;
            wrapped = true;
        }

        if (wrapped)
        {
            transform.position = pos;
        }
    }

    /// <summary>
    /// Called when asteroid collides with laser
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            Destroy(collision.gameObject);
            DestroyAsteroid();
        }
    }

    /// <summary>
    /// Destroys the asteroid and awards bonus score
    /// </summary>
    private void DestroyAsteroid()
    {
        // Award bonus score for destroying asteroid
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(destroyBonus);
            Debug.Log($"Asteroid destroyed! Bonus score added: +{destroyBonus}");
        }
        else
        {
            Debug.LogWarning("ScoreManager.Instance is null! Cannot add bonus score.");
        }

        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
