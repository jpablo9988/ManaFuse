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

    [Tooltip("The amount of time in seconds that the projectile won't hit the shooter")] [SerializeField]
    private float collisionDelay = 0.3f;

    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        //Move forward in the local forward direction.
        if (isEnemyProjectile)
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }
        else //This is the worst bandaid fix of my life...
        {
            transform.Translate(-speed * Time.deltaTime * Vector3.left);
        }
        
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
            }
            Destroy(gameObject);
            return;
        }
        
        //If the projectile hits an enemy:
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }
        
        //For everything else, yeet the projectile
        Destroy(gameObject);
    }
}
