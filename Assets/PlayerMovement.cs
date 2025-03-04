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
   
        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate()
    {

    }
}
