using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    [Header("Sprint Settings")]
    [SerializeField] private float sprintDistance = 3f;
    [SerializeField] private float sprintDuration = 0.2f;
    [SerializeField] private float sprintCooldown = 1f;
    [SerializeField] private AnimationCurve sprintCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Dependencies")]
    [SerializeField] private Transform targetTransformRotation;
    [SerializeField] private CharacterAnimationManager _charAnimations;

    private InputAction moveAction;
    private InputAction sprintAction;
    private Vector2 moveInput;
    private Rigidbody rb;
    private Vector2 _moveInput;
    private float roundedAngle = 0.0f;
    private bool isSprinting;
    private float sprintTimer;
    private float sprintCooldownTimer;
    private Vector3 sprintStartPosition;
    private Vector3 sprintTargetPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        moveAction.Enable();
        sprintAction.Enable();
    }

    private void OnDestroy()
    {
        if (moveAction != null) moveAction.Disable();
        if (sprintAction != null) sprintAction.Disable();
    }


    void Update()
    {
        //this.transform.position = this._rigidbody.transform.position;
        moveInput = moveAction.ReadValue<Vector2>();

        if (sprintAction.triggered && !isSprinting && sprintCooldownTimer <= 0)
        {
            InitiateSprint();
        }

        if (isSprinting)
        {
            UpdateSprint();
        }
        else
        {
            Rotate();
            if (sprintCooldownTimer > 0)
                sprintCooldownTimer -= Time.deltaTime;
        }
        _charAnimations.SetAnimationParameters(_moveInput.magnitude, roundedAngle);

    }
    private void FixedUpdate()
    {
        if (!isSprinting)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 movement = new(moveInput.x, 0f, moveInput.y);
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (moveInput.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            roundedAngle = Mathf.Round(angle / 45f) * 45f;
            Quaternion targetRotation = Quaternion.Euler(0f, roundedAngle - 90f, 0f);
            targetTransformRotation.rotation = targetRotation;
        }
    }

    private void InitiateSprint()
    {
        // Only sprint if there's movement input
        if (moveInput.magnitude < 0.1f) return;

        isSprinting = true;
        sprintTimer = 0f;
        sprintStartPosition = transform.position;

        // Use movement input direction instead of transform.forward
        Vector3 sprintDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        sprintTargetPosition = sprintStartPosition + sprintDirection * sprintDistance;
        // Raycast to prevent sprinting through walls
        if (Physics.Raycast(sprintStartPosition, sprintDirection, out RaycastHit hit, sprintDistance))
        {
            sprintTargetPosition = hit.point - (sprintDirection * 0.5f);
        }
    }

    private void UpdateSprint()
    {
        sprintTimer += Time.deltaTime;
        float normalizedTime = sprintTimer / sprintDuration;
        if (normalizedTime >= 1f)
        {
            // End sprint
            isSprinting = false;
            sprintCooldownTimer = sprintCooldown;
            rb.position = sprintTargetPosition;
        }
        else
        {
            // Interpolate position using the sprint curve
            float curveValue = sprintCurve.Evaluate(normalizedTime);
            rb.position = Vector3.Lerp(sprintStartPosition, sprintTargetPosition, curveValue);
        }
    }
}
