using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the spaceship movement using arrow keys or WASD
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 300f;

    [Header("Boundaries")]
    [SerializeField] private float boundaryX = 8f;
    [SerializeField] private float boundaryY = 4.5f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private float currentMoveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }
    }

    private void Update()
    {
        UpdateMoveSpeed();
        HandleMovement();
        ClampPosition();
    }

    /// <summary>
    /// Updates move speed with power-up bonuses
    /// </summary>
    private void UpdateMoveSpeed()
    {
        currentMoveSpeed = baseMoveSpeed;
        
        if (PowerUpManager.Instance != null)
        {
            currentMoveSpeed += PowerUpManager.Instance.CurrentSpeedBonus;
        }
    }

    /// <summary>
    /// Handles player movement input
    /// </summary>
    private void HandleMovement()
    {
        // Get input from arrow keys or WASD
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        moveInput = new Vector2(horizontal, vertical).normalized;

        // Move the spaceship
        if (moveInput.magnitude > 0)
        {
            rb.linearVelocity = moveInput * currentMoveSpeed;

            // Rotate spaceship to face movement direction
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    /// <summary>
    /// Keeps the player within screen boundaries
    /// </summary>
    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -boundaryX, boundaryX);
        pos.y = Mathf.Clamp(pos.y, -boundaryY, boundaryY);
        transform.position = pos;
    }

    /// <summary>
    /// Called when the player is destroyed
    /// </summary>
    private void OnDestroy()
    {
        // Game over is now handled by HealthManager
        // This prevents double game over triggers
    }
}