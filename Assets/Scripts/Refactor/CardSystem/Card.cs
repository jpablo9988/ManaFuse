using UnityEngine;

namespace CardSystem
{
    /// <summary>
    /// Defines a card in the game with various properties and type-specific behaviors.
    /// Each card can be an Attack, Heal, or Dash type, with different effects when activated.
    /// Cards are ScriptableObjects that can be created and configured in the Unity editor.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCard", menuName = "Card System/Card")]
    public sealed class Card : ScriptableObject
    {
        [Tooltip("The display name of the card.")]
        public string cardName;

        [Tooltip("The icon displayed in the card UI.")]
        public Sprite cardIcon;

        [Tooltip("The card type that determines its behavior: Attack, Heal, or Dash.")]
        public CardType cardType;

        [Tooltip("The mana cost to use this card.")]
        public int cardCost;

        [Tooltip("The projectile prefab to spawn for Attack cards. Leave null for non-projectile cards.")]
        public GameObject projectilePrefab;

        [Tooltip("Optional particle effect prefab to spawn when card is activated.")]
        public GameObject particleEffectPrefab;

        [Tooltip("Whether this card should spawn a projectile when activated (Attack cards only).")]
        public bool spawnProjectile = true;

        [Tooltip("Whether this card should spawn its particle effect when activated.")]
        public bool spawnParticleEffect = false;

        // Type-specific fields
        [Header("Heal Card Settings")]
        [Tooltip("Amount of mana units to restore when a Heal card is activated.")]
        [SerializeField] private int healAmount = 2;

        [Header("Dash Card Settings")]
        [Tooltip("Distance in units the player will dash when a Dash card is activated.")]
        [SerializeField] private float dashDistance = 3f;

        [Tooltip("Duration in seconds of the dash movement.")]
        [SerializeField] private float dashDuration = 0.2f;

        /// <summary>
        /// Activates the card's effect based on its type.
        /// First applies the mana cost, then executes type-specific behavior,
        /// and optionally spawns a particle effect.
        /// </summary>
        /// <param name="user">The GameObject that is using the card (typically the player).</param>
        public void Activate(GameObject user)
        {
            Debug.Log($"{cardName} activated!");

            // Apply mana cost to the player
            GameContext.Instance.Player.ChangeMana(-cardCost, false);

            // Execute type-specific behavior
            switch (cardType)
            {
                case CardType.Attack:
                    ActivateAttackEffect(user);
                    break;

                case CardType.Heal:
                    ActivateHealEffect(user);
                    break;

                case CardType.Dash:
                    ActivateDashEffect(user);
                    break;
            }

            // Spawn particle effect if enabled
            if (spawnParticleEffect && particleEffectPrefab != null)
            {
                Instantiate(particleEffectPrefab, user.transform.position, user.transform.rotation);
            }
        }
        public void OnHitTarget()
        {
            if (cardType == CardType.Attack)
            {
                GameContext.Instance.Player.ChangeMana(this.cardCost, true);
            }
        }

        /// <summary>
        /// Executes the Attack card's effect: spawning a projectile.
        /// </summary>
        /// <param name="user">The GameObject that is using the card.</param>
        private void ActivateAttackEffect(GameObject user)
        {
            // Attack cards can shoot projectiles
            if (!spawnProjectile || !projectilePrefab) return;
            if (projectilePrefab.TryGetComponent(out Projectile projectileRef))
            {
                GameContext.Instance.ProjectileManager.ShootProjectile(projectileRef);
            }
        }

        /// <summary>
        /// Executes the Heal card's effect: restoring mana to the player.
        /// </summary>
        /// <param name="user">The GameObject that is using the card.</param>
        private void ActivateHealEffect(GameObject user)
        {
            Debug.Log($"Healing for {healAmount} mana units from {cardName}");
            // Add mana to the player using the ChangeMana method
            // The true parameter means it affects both sliders (green and red)
            GameContext.Instance.Player.ChangeMana(healAmount, true);
        }

        /// <summary>
        /// Executes the Dash card's effect: moving the player quickly in their facing direction.
        /// Uses the player's current facing angle to determine the dash direction.
        /// </summary>
        /// <param name="user">The GameObject that is using the card.</param>
        private void ActivateDashEffect(GameObject user)
        {
            if (GameContext.Instance.Player.PlayerMovementManager != null)
            {
                var playerMovement = GameContext.Instance.Player.PlayerMovementManager;

                // Get the player's current facing angle
                float angle = playerMovement.RoundedAngle * Mathf.Deg2Rad;

                // Convert angle to a direction vector
                // Using the snapped rotation to match visual facing
                Vector2 moveInput = new Vector2(
                    Mathf.Sin(angle),  // x component
                    Mathf.Cos(angle)   // z component (as y in Vector2)
                );

                Debug.Log($"Dashing with angle {playerMovement.RoundedAngle}, direction: {moveInput}");

                // Use the direction vector for the sprint
                playerMovement.InitiateSprint(
                    moveInput,
                    dashDistance,
                    dashDuration
                );
            }
        }
    }

    /// <summary>
    /// Defines the possible card types that determine a card's behavior.
    /// </summary>
    public enum CardType
    {
        /// <summary>Attack cards shoot projectiles.</summary>
        Attack,

        /// <summary>Heal cards restore mana to the player.</summary>
        Heal,

        /// <summary>Dash cards move the player quickly in their facing direction.</summary>
        Dash
    }
}