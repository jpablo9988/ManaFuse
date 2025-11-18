using UnityEngine;
using CardSystem;
using UnityEngine.Serialization;
using System;

/// <summary>
/// TODO: Enemy Movement and Enemy Stats (or just NPC/Enemy Class) should be separate.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("If true, enemy detection is triggered when the player is within the minimum range. If false, detection uses the maximum range.")]
    public bool detectionUsesMinRange = false;
    [Header("Attack Settings")]
    [Tooltip("The card used for the enemy's attack; variables from this card determine the projectile.")]
    public Card attackCard;
    [Tooltip("The minimum distance the enemy wants to be from the player.")]
    public float optimalRangeMin = 6f;
    [Tooltip("The maximum distance the enemy wants to be from the player.")]
    public float optimalRangeMax = 12f;
    [Tooltip("Time (in seconds) between enemy attacks.")]
    public float attackCooldown = 0.7f;
    [Tooltip("Base speed at which the enemy moves toward/away from the player.")]
    public float moveSpeed = 6f;
    [Tooltip("The amount of damage the enemy's projectile will deal.")]
    public float projectileDamage = 10f;
    [Tooltip("TEMPORARY: Makes an enemy static")]
    public bool isStatic = false;

    [FormerlySerializedAs("oscillationMagnitude")]
    [Header("Maneuver Settings")]
    [Tooltip("Jiggle movement magnitude")]
    public float jiggleMagnitude = 5f;
    [FormerlySerializedAs("oscillationSpeed")]
    [Tooltip("Jiggle movement speed")]
    public float jiggleSpeed = 5f;

    [Header("Health Settings")]
    [Tooltip("The total health of the enemy.")]
    public float health = 100f;
    [Tooltip("Amount of mana to reward the player when the enemy dies.")]
    public float manaReward = 10f;
    [Header("Dependencies")]
    [SerializeField]
    [Tooltip("(Optional) Where the attack bullet with spawn from.")]
    private Transform _attackSpawnPoint;

    //Reference to the player's transform.
    private Transform playerTransform;
    //Records time of the last attack.
    private float lastAttackTime = -Mathf.Infinity;
    //Stores the enemy's initial Y coordinate.
    private float initialY;
    public static event Action OnDeathEnemy;
    private Rigidbody rb;

    private void Start()
    {
        initialY = transform.position.y;
        if (GameContext.Instance.Player)
        {
            playerTransform = GameContext.Instance.Player.transform;
        }
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!playerTransform || isStatic)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        //Determine the detection threshold based on detection mode.
        float detectionThreshold = detectionUsesMinRange ? optimalRangeMin : optimalRangeMax;

        //Sit and idle until the player comes within range.
        if (distanceToPlayer > detectionThreshold)
        {
            //Reset Y position to prevent vertical drift.
            SetYFixed();
            return;
        }
        Vector3 newLinearVelocity = new();
        //Move to maintain optimal range.
        if (distanceToPlayer < optimalRangeMin)
        {
            //Too close: move away.
            Vector3 moveDirection = (transform.position - playerTransform.position).normalized;
            moveDirection.y = 0;
            //transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            newLinearVelocity = moveDirection * moveSpeed;

        }
        else if (distanceToPlayer > optimalRangeMax)
        {
            //Too far: move toward the player.
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
            moveDirection.y = 0;
            //transform.position += moveDirection * (moveSpeed * Time.deltaTime);
            newLinearVelocity = moveDirection * moveSpeed;
        }

        //Add random motion to simulate dodging / avoiding projectiles.
        Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
        toPlayer.y = 0;
        Vector3 right = Vector3.Cross(toPlayer, Vector3.up);
        //Lateral jiggly wigglies.
        float lateralOffset = Mathf.Sin(Time.time * jiggleSpeed) * jiggleMagnitude;
        //Small forward/back jiggly wigglies.
        float forwardOffset = Mathf.Sin(Time.time * jiggleSpeed * 0.5f) * jiggleMagnitude * 0.5f;
        Vector3 jiggles = right * lateralOffset + toPlayer * forwardOffset;
        jiggles.y = 0;
        //transform.position += jiggles * Time.deltaTime;
        rb.linearVelocity = newLinearVelocity + jiggles;

        //Attack if within optimal range and cooldown has elapsed.
        if (distanceToPlayer < optimalRangeMin ||
            distanceToPlayer > optimalRangeMax ||
            Time.time - lastAttackTime < attackCooldown)
        {
            SetYFixed();
            return;
        }

        Attack();
        lastAttackTime = Time.time;

        //Ensure Y remains fixed.
        SetYFixed();
    }


    private void Attack()
    {
        if (!attackCard)
            return;

        if (attackCard.spawnProjectile && attackCard.projectilePrefab)
        {
            //Determine the direction toward the player.
            var direction = (playerTransform.position - transform.position).normalized;
            var rotation = Quaternion.LookRotation(direction);

            //Instantiate the projectile at the enemy's position.
            GameObject projObj;
            if (_attackSpawnPoint != null)
            {
                projObj = Instantiate(attackCard.projectilePrefab, _attackSpawnPoint.position, transform.rotation);
            }
            else
            {
                projObj = Instantiate(attackCard.projectilePrefab, transform.position, rotation);
            }
            var proj = projObj.GetComponent<Projectile>(); // Ensure the prefab has a Projectile component.
            if (proj)
            {
                //Ensure the projectile can damage the player.
                proj.isEnemyProjectile = true;
                proj.selfDamageEnabled = true; // Assurance that the projectile deals damage.
                proj.damage = projectileDamage;
            }
        }
        if (attackCard.spawnParticleEffect && attackCard.particleEffectPrefab)
        {
            if (_attackSpawnPoint != null)
            {
                Instantiate(attackCard.particleEffectPrefab, _attackSpawnPoint.position, transform.rotation);
            }
            else
            {
                Instantiate(attackCard.particleEffectPrefab, transform.position, transform.rotation);
            }
        }
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
            Die();
        print("Enemy hit for: " + damage);
    }


    private void Die()
    {
        //Reward player for the kill.
        GameContext.Instance.Player.ChangeManaByTickUnit(manaReward);
        OnDeathEnemy?.Invoke();
        Destroy(gameObject);
    }


    private void SetYFixed()
    {
        var pos = transform.position;
        pos.y = initialY;
        transform.position = pos;
    }
}
