using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private CharacterAnimationManager _characterAnimations;
    // --------------------Input Variables ---------------------- //
    private PlayerInputMap _inputActions;
    private PlayerInputMap.PlayerActions _playerActions;
    private InputAction _movementAction;
    private InputAction _sprintAction; //TEMPORARY: Should be a Card Instead.

    // ---------------------------------------------------------//

    private Vector2 _moveInput = new();
    public Vector2 CurrentMovementInput => _moveInput;


    private void Awake()
    {
        _inputActions = new PlayerInputMap();
        _playerActions = _inputActions.Player;
        _movementAction = _playerActions.Move;
        _sprintAction = _playerActions.Sprint;
        if (_playerMovement == null) _playerMovement = GetComponent<PlayerMovement>();
        if (_characterAnimations == null) _characterAnimations = GetComponent<CharacterAnimationManager>();
        _characterAnimations.SetAnimationParameters(0, _playerMovement.RoundedAngle);
        //Start Facing Down.
        _characterAnimations.AnimationSpeed(0);
    }
    void OnEnable()
    {
        _inputActions.Player.Enable();
        _sprintAction.started += Sprint; //Should only trigger once. (When _sprintAction is pressed)
        _characterAnimations.AnimationSpeed(1);
    }

    void OnDisable()
    {
        _inputActions.Player.Disable();
        _sprintAction.started -= Sprint;
    }
    void Update()
    {
        _moveInput = _movementAction.ReadValue<Vector2>();
        if (!_playerMovement.IsSprinting)
        {
            if (_moveInput.magnitude > 0)
            {
                _playerMovement.Rotate(_moveInput);
            }
            _characterAnimations.SetAnimationParameters(_moveInput.magnitude, _playerMovement.RoundedAngle);
        }
    }
    void FixedUpdate()
    {
        if (!_playerMovement.IsSprinting) _playerMovement.Move(_moveInput);
    }
    public void Sprint()
    {
        _playerMovement.InitiateSprint(_moveInput);
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _playerMovement.InitiateSprint(_moveInput);
        }
    }

}
