using UnityEngine;

public class AttackManager : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField]
    private Projectile projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private CharacterAnimationManager _animatorManager;

    public void ShootProjectile(Projectile projectilePrefab)
    {
        // Use the provided prefab if available, otherwise use a default
        //GameObject prefabToUse = projectilePrefab != null ? projectilePrefab : defaultProjectilePrefab;

        // Instantiate the projectile at the appropriate position/rotation
        Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        _animatorManager.Shoot();
    }
}
