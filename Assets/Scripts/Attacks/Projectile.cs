using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles projectile movement and collision. The projectile will deal damage to the player
/// if either it is flagged as an enemy projectile or if self damage is enabled (for player–fired projectiles),
/// and it will also damage enemies upon collision.
/// </summary>
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Tooltip("The speed of the projectile in units per second.")]
    [SerializeField] private float speed = 20f;
    [Tooltip("The time in seconds before the projectile is destroyed.")]
    [SerializeField] private float lifetime = 3f;
    [Tooltip("The amount of damage the projectile will deal.")]
    public float damage = 10f;
    [Tooltip("True if the enemy has fired this projectile")]
    public bool isEnemyProjectile = false;
    [Tooltip("True if the player can damage themselves with this projectile.")]
    public bool selfDamageEnabled = false;

    [Tooltip("The amount of time in seconds that the projectile won't hit the shooter")]
    [SerializeField]
    private float collisionDelay = 0.3f;
    public static event Action OnHitEnemy;
    private float spawnTime;
    [Header("Dependencies")]
    [SerializeField]
    [Tooltip("Particle System of the Bullet that plays on impact. Can be left blank if there's none. ")]
    private ParticleSystem impactParticleSystem;
    [SerializeField]
    [Tooltip("Will try to get one from current gO if there's none assigned. Can be left blank if there's none. ")]
    private TrailRenderer bulletTrail;
    [SerializeField]
    [Tooltip("Important if using particle systems when being destroyed. Will try and get its attached component from the current GameObject. ")]
    private MeshRenderer bulletVisuals;
    void Awake()
    {
        if (bulletTrail == null)
            TryGetComponent(out bulletTrail);
        if (bulletVisuals == null)
            TryGetComponent(out bulletVisuals);
    }
    private void Start()
    {
        spawnTime = Time.time;
        Invoke(nameof(BulletOnNoLifespan), lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (Time.time - spawnTime < collisionDelay)
            return;


        //If the projectile hits the player:
        if (collision.gameObject.CompareTag("Player"))
        {
            //Damage if it is an enemy projectile or self-damage is enabled.
            if (isEnemyProjectile || selfDamageEnabled)
            {
                //Reduce player's mana by the projectile's damage.
                GameContext.Instance.Player.ChangeManaByTickUnit(-damage, true);
                BulletOnImpact();
            }
            return;
        }

        //If the projectile hits an enemy:
        if (collision.gameObject.CompareTag("Enemy") && !isEnemyProjectile)
        {
            if (collision.gameObject.TryGetComponent<EnemyAI>(out var enemy))
            {
                enemy.TakeDamage(damage);
                //Activate on Hit Enemy.
                OnHitEnemy?.Invoke();
                //This is a one-time effect.
                OnHitEnemy = null;
            }
            BulletOnImpact();
            return;
        }
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Wall"))
        {
            //For everything blockable, yeet the projectile
            BulletOnImpact();
        }
    }
    //Disables the script and the bullet's visuals and starts the particle impact.
    private void BulletOnImpact()
    {
        if (impactParticleSystem != null && bulletVisuals != null)
        {
            bulletVisuals.enabled = false;
            if (bulletTrail != null) bulletTrail.enabled = false;
            impactParticleSystem.Play();
            this.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void BulletOnNoLifespan()
    {
        if (bulletTrail != null && bulletVisuals != null)
        {
            bulletVisuals.enabled = false;
            bulletTrail.autodestruct = true;
            this.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
