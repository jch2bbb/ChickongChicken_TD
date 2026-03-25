using UnityEngine;

// Attach this script to each invisible boundary wall object.
// When a bullet enters the boundary collider, it is destroyed.
public class BulletBoundary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
        }
    }
}