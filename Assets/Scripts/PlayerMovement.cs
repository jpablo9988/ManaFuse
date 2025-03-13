using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [Tooltip("Will it rotate its own rigidbody or a targeted transform?")]
    [SerializeField] private bool rotateRigidbody = false;
    [Header("Dependencies")]
    [SerializeField] private Transform targetTransformRotation;
    [SerializeField] private CharacterAnimationManager _charAnimations;
    [SerializeField] private Rigidbody _rigidbody;


    private Vector2 _moveInput;

    private float roundedAngle = 0.0f;

    private void Awake()
    {
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
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
    void Update()
    {
        _charAnimations.SetAnimationParameters(_moveInput.magnitude, roundedAngle);
        //this.transform.position = this._rigidbody.transform.position;

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
        _rigidbody.MovePosition(_rigidbody.position + moveSpeed * Time.fixedDeltaTime * movement);
    }

    private void Rotate()
    {
        // Only rotate if there is input
        if (!(_moveInput.magnitude > 0.1f)) return;
        // Calculate the angle from input
        var angle = Mathf.Atan2(_moveInput.x, _moveInput.y) * Mathf.Rad2Deg;

        // Round to nearest 45 degrees
        roundedAngle = Mathf.Round(angle / 45f) * 45f;
        Debug.Log(roundedAngle);
        // Create rotation with rounded angle on Y axis
        // Add 90 degrees offset because forward is Z axis
        var targetRotation = Quaternion.Euler(0f, roundedAngle - 90f, 0f);

        // Instantly set rotation instead of smooth rotation for grid-like movement
        if (rotateRigidbody || !targetTransformRotation)
        {
            _rigidbody.MoveRotation(targetRotation);
        }
        else
        {
            targetTransformRotation.rotation = targetRotation;
        }
    }
}
