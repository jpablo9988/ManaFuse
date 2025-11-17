using System.Collections;
using UnityEngine;

/// <summary>
/// Handles player movement, rotation, and dash/sprint abilities.
/// Uses a Rigidbody for physics-based movement and implements 8-directional rotation snapping.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, ICharacterMovement
{
    [Header("Movement Settings")]
    [Tooltip("Base movement speed of the player.")]
    [SerializeField] private float moveSpeed = 1000f;

    [Header("Sprint Settings")]
    [Tooltip("Distance in units the player will move during a default sprint.")]
    [SerializeField] private float sprintDistance = 3f;

    [Tooltip("Duration in seconds of the default sprint movement.")]
    [SerializeField] private float sprintDuration = 0.2f;
    
    [Tooltip("Duration in seconds of the sprint cooldown.")]
    [SerializeField] private float sprintCooldown = 2f;

    [Tooltip("Animation curve that controls sprint movement over time. 0,0 to 1,1 range.")]
    [SerializeField] private AnimationCurve sprintCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Dependencies")]
    [Tooltip("Transform that will be rotated to match the player's facing direction.")]
    [SerializeField] private Transform targetTransformRotation;

    /// <summary>
    /// Reference to the player's Rigidbody component for physics-based movement.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// The current rotation angle of the player, snapped to 45-degree increments.
    /// </summary>
    private float _roundedAngle = 180.0f;

    /// <summary>
    /// Whether the player is currently performing a sprint/dash.
    /// </summary>
    private bool _isSprinting;

    /// <summary>
    /// Gets whether the player is currently sprinting.
    /// </summary>
    public bool IsSprinting => _isSprinting;

    /// <summary>
    /// Last time player sprinted.
    /// </summary>
    private float _lastSprintTime = 0.0f;

    /// <summary>
    /// Gets the current visual rotation angle of the player between -180 and 180 degrees.
    /// This is snapped to 45-degree increments for 8-directional movement.
    /// </summary>
    public float RoundedAngle => _roundedAngle;

    /// <summary>
    /// Gets required components on initialization.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Moves the player based on input direction.
    /// Should be called from FixedUpdate for physics-based movement.
    /// </summary>
    /// <param name="m_Input">Movement input direction as a Vector2 (x,z).</param>
    public void Move(Vector2 m_Input)
    {
        Vector3 movement = new(m_Input.x, 0f, m_Input.y);
        if (m_Input.magnitude == 0) rb.linearVelocity = new Vector3(0, 0, 0);
        rb.linearVelocity = moveSpeed * movement;
    }

    /// <summary>
    /// Rotates the player based on input direction.
    /// Snaps rotation to 45-degree increments (8 directions).
    /// Should be called from Update, not FixedUpdate, as it does not use physics.
    /// </summary>
    /// <param name="m_Input">Movement input direction as a Vector2 (x,z).</param>
    public void Rotate(Vector2 m_Input)
    {
        if (m_Input.magnitude > 0.1f)
        {
            // Calculate angle from input
            float angle = Mathf.Atan2(m_Input.x, m_Input.y) * Mathf.Rad2Deg;

            // Snap to 45-degree increments (0, 45, 90, 135, 180, 225, 270, 315)
            _roundedAngle = Mathf.Round(angle / 45f) * 45f;

            // Apply rotation to the target transform (usually the visual model)
            Quaternion targetRotation = Quaternion.Euler(0f, _roundedAngle - 90f, 0f);
            targetTransformRotation.rotation = targetRotation;
        }
    }

    /// <summary>
    /// Initiates a sprint using the default sprint settings.
    /// </summary>
    /// <param name="m_Input">Movement input direction as a Vector2 (x,z).</param>
    public void InitiateSprint(Vector2 m_Input)
    {
        InitiateSprint(m_Input, sprintDistance, sprintDuration);
    }

    /// <summary>
    /// Initiates a sprint with custom distance and duration parameters.
    /// Used by Dash cards to create dashes with different properties.
    /// </summary>
    /// <param name="m_Input">Movement input direction as a Vector2 (x,z).</param>
    /// <param name="distance">The distance to sprint in units.</param>
    /// <param name="duration">The duration of the sprint in seconds.</param>
    public void InitiateSprint(Vector2 m_Input, float distance, float duration)
    {
        // Don't sprint if there's no input direction or already sprinting
        if (m_Input.magnitude < 0.1f || _isSprinting) return;
        
        // Check if the time since last sprint is more than the sprint cooldown
        if (Time.time - _lastSprintTime < sprintCooldown) return;
        
        // Set the last sprint time
        _lastSprintTime = Time.time;

        // Mark as sprinting to prevent movement during sprint
        _isSprinting = true;

        // Calculate start and end positions
        Vector3 sprintStartPosition = transform.position;
        Vector3 sprintDirection = new Vector3(m_Input.x, 0f, m_Input.y).normalized;
        Vector3 sprintTargetPosition = sprintStartPosition + sprintDirection * distance;

        // Raycast to prevent sprinting through walls
        if (Physics.Raycast(sprintStartPosition, sprintDirection, out RaycastHit hit, distance))
        {
            // If we hit something, stop short with a small margin
            sprintTargetPosition = hit.point - (sprintDirection * 0.5f);
        }

        // Start the sprint coroutine
        StartCoroutine(UpdateSprint(duration, sprintStartPosition, sprintTargetPosition));
    }

    /// <summary>
    /// Coroutine that handles the sprint movement over time.
    /// Uses an animation curve to control the movement speed.
    /// </summary>
    /// <param name="sprintDuration">Duration of the sprint in seconds.</param>
    /// <param name="sprintStartPosition">Starting position of the sprint.</param>
    /// <param name="sprintTargetPosition">Target position of the sprint.</param>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator UpdateSprint(float sprintDuration, Vector3 sprintStartPosition, Vector3 sprintTargetPosition)
    {
        float sprintTimer = 0.0f;
        float normalizedTime = 0.0f;

        // Continue until we reach the end of the sprint
        while (normalizedTime <= 1f)
        {
            sprintTimer += Time.deltaTime;
            normalizedTime = sprintTimer / sprintDuration;

            // Use animation curve to control movement speed
            float curveValue = sprintCurve.Evaluate(normalizedTime);

            // Lerp position based on curve
            rb.position = Vector3.Lerp(sprintStartPosition, sprintTargetPosition, curveValue);

            yield return null;
        }

        // End sprint
        _isSprinting = false;
        rb.position = sprintTargetPosition;
    }
    
    //Death Movement Fix
    private void OnEnable()
    {
        PlayerManager.OnDeathPlayer += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        PlayerManager.OnDeathPlayer -= HandlePlayerDeath;
    }
    private void HandlePlayerDeath()
    {
        StopAllMovement();
        enabled = false;
    }
    public void StopAllMovement()
    {
        StopAllCoroutines();
        _isSprinting = false;
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
