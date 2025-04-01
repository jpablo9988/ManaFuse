using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, ICharacterMovement
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Sprint Settings")]
    [SerializeField] private float sprintDistance = 3f;
    [SerializeField] private float sprintDuration = 0.2f;
    [SerializeField] private AnimationCurve sprintCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Dependencies")]
    [SerializeField] private Transform targetTransformRotation;
    private Rigidbody rb;
    private float _roundedAngle = 0.0f;
    private bool _isSprinting;
    public bool IsSprinting => _isSprinting;
    /// <summary>
    /// The current visual rotation from the character between -180 and 180 degrees.
    /// </summary>
    public float RoundedAngle => _roundedAngle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Move(Vector2 m_Input)
    {
        Vector3 movement = new(m_Input.x, 0f, m_Input.y);
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
    }

    /// <summary>
    /// Visually Rotate the player. Should be run inside an Update instead of a FixedUpdate
    /// as it does not use the physics system.
    /// </summary>
    /// <param name="m_Input"> Movement Input Parameters.</param>
    public void Rotate(Vector2 m_Input)
    {
        if (m_Input.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(m_Input.x, m_Input.y) * Mathf.Rad2Deg;
            _roundedAngle = Mathf.Round(angle / 45f) * 45f;
            Quaternion targetRotation = Quaternion.Euler(0f, _roundedAngle - 90f, 0f);
            targetTransformRotation.rotation = targetRotation;
        }
    }

    public void InitiateSprint(Vector2 m_Input)
    {
        if (m_Input.magnitude < 0.1f || _isSprinting) return;
        _isSprinting = true;
        Vector3 sprintStartPosition = transform.position;
        Vector3 sprintDirection = new Vector3(m_Input.x, 0f, m_Input.y).normalized;
        Vector3 sprintTargetPosition = sprintStartPosition + sprintDirection * sprintDistance;
        // Raycast to prevent sprinting through walls
        if (Physics.Raycast(sprintStartPosition, sprintDirection, out RaycastHit hit, sprintDistance))
        {
            sprintTargetPosition = hit.point - (sprintDirection * 0.5f);
        }
        StartCoroutine(UpdateSprint(sprintDuration, sprintStartPosition, sprintTargetPosition));
    }
    private IEnumerator UpdateSprint(float sprintDuration, Vector3 sprintStartPosition, Vector3 sprintTargetPosition)
    {
        float sprintTimer = 0.0f;
        float normalizedTime = 0.0f;
        while (normalizedTime <= 1f)
        {
            sprintTimer += Time.deltaTime;
            normalizedTime = sprintTimer / sprintDuration;
            float curveValue = sprintCurve.Evaluate(normalizedTime);
            rb.position = Vector3.Lerp(sprintStartPosition, sprintTargetPosition, curveValue);
            yield return null;
        }
        // End sprint
        _isSprinting = false;
        rb.position = sprintTargetPosition;
    }
}
