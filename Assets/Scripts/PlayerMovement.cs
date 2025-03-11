using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [Tooltip("Will it rotate its own rigidbody or a targeted transform?")]
    [SerializeField] private bool rotateRigidbody = false;
    [Header("Dependencies")]
    [SerializeField] private Transform targetTransformRotation;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown = 0.5f;
    private float _lastShootTime;

    private Vector2 _moveInput;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        InputManager.OnMoveInteracted += ReadMovementInput;
    }

    private void OnDisable()
    {
        InputManager.OnMoveInteracted -= ReadMovementInput;
    }

    private void ReadMovementInput(Vector2 input)
    {
        _moveInput = input;
    }

    private void HandleShooting()
    {
        if (Time.time - _lastShootTime < shootCooldown) return;
        
        if (projectilePrefab != null && shootPoint != null)
        {
            Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            _lastShootTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        // Use world space movement instead of local space
        Vector3 movement = new(_moveInput.x, 0f, _moveInput.y);
        _rb.MovePosition(_rb.position + moveSpeed * Time.fixedDeltaTime * movement);
    }

    private void Rotate()
    {
        // Only rotate if there is input
        if (!(_moveInput.magnitude > 0.1f)) return;
        // Calculate the angle from input
        var angle = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg;

        // Round to nearest 45 degrees
        var roundedAngle = Mathf.Round(angle / 45f) * 45f;

        // Create rotation with rounded angle on Y axis
        // Add 90 degrees offset because forward is Z axis
        var targetRotation = Quaternion.Euler(0f, roundedAngle - 90f, 0f);

        // Instantly set rotation instead of smooth rotation for grid-like movement
        if (rotateRigidbody || !targetTransformRotation)
        {
            _rb.MoveRotation(targetRotation);
        }
        else
        {
            targetTransformRotation.rotation = targetRotation;
        }
    }
}
