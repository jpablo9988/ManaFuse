using UnityEngine;

public class AttackManager : MonoBehaviour
{

    [Header("Dependencies")]
    private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;

    public void ShootProjectile(GameObject projectilePrefab)
    {
        // Use the provided prefab if available, otherwise use a default
        //GameObject prefabToUse = projectilePrefab != null ? projectilePrefab : defaultProjectilePrefab;
        
        // Instantiate the projectile at the appropriate position/rotation
        Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
    }
}
