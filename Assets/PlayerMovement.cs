using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;

    private InputAction moveAction;
    private Vector2 moveInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Get reference to the Move action
        moveAction = InputSystem.actions.FindAction("Move");
        
        // Enable the action
        moveAction.Enable();
    }

    private void OnDestroy()
    {
        // Clean up by disabling the action when the object is destroyed
        if (moveAction != null)
            moveAction.Disable();
    }

    private void Update()
    {
        // Read the movement input value
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        // Use world space movement instead of local space
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        // Only rotate if there is input
        if (moveInput.magnitude > 0.1f)
        {
            // Calculate the angle from input
            float angle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg;
            
            // Round to nearest 45 degrees
            float roundedAngle = Mathf.Round(angle / 45f) * 45f;
            
            // Create rotation with rounded angle on Y axis
            // Add 90 degrees offset because forward is Z axis
            Quaternion targetRotation = Quaternion.Euler(0f, roundedAngle - 90f, 0f);
            
            // Instantly set rotation instead of smooth rotation for grid-like movement
            rb.MoveRotation(targetRotation);
        }
    }
}
