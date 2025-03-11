using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private float _lastShootTime;

    [Header("Dependencies")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [Header("Settings")]
    [SerializeField] private float shootCooldown = 0.5f;
    public void ShootProjectile()
    {
        if (Time.time - _lastShootTime < shootCooldown) return;

        if (projectilePrefab != null && shootPoint != null)
        {
            Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            _lastShootTime = Time.time;
        }
    }
}
