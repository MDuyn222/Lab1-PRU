using UnityEngine;

/// <summary>
/// Handles player shooting mechanics
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float baseFireRate = 0.3f;
    [SerializeField] private float laserSpeed = 10f;

    private float nextFireTime = 0f;
    private float currentFireRate;

    private void Update()
    {
        UpdateFireRate();
        
        // Check for shooting input (Space key or Mouse click)
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + currentFireRate;
        }
    }

    /// <summary>
    /// Updates fire rate with power-up bonuses
    /// </summary>
    private void UpdateFireRate()
    {
        currentFireRate = baseFireRate;
        
        if (PowerUpManager.Instance != null)
        {
            // Reduce fire rate cooldown (faster shooting)
            currentFireRate = Mathf.Max(0.05f, currentFireRate - PowerUpManager.Instance.CurrentFireRateBonus);
        }
    }

    /// <summary>
    /// Shoots a laser projectile
    /// </summary>
    private void Shoot()
    {
        if (laserPrefab == null)
        {
            Debug.LogWarning("Laser prefab not assigned!");
            return;
        }

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + transform.up * 0.5f;
        GameObject laser = Instantiate(laserPrefab, spawnPosition, transform.rotation);
        
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        if (laserRb != null)
        {
            laserRb.linearVelocity = transform.up * laserSpeed;
        }

        // Destroy laser after 3 seconds
        Destroy(laser, 3f);
    }
}