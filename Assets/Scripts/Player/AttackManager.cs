using UnityEngine;

public class AttackManager : MonoBehaviour
{

    [Header("Dependencies")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;

    public void ShootProjectile()
    {
        Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
    }
}
