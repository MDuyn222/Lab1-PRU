using UnityEngine;

/// <summary>
/// Laser projectile behavior
/// </summary>
public class Laser : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        // Destroy laser when it goes off screen
        Destroy(gameObject);
    }
}