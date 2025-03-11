using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    [Header("Start Settings")]
    [SerializeField]
    private bool _activatePlayerInputsOnEnable = true;
    [SerializeField]
    private bool _activateUIInputsOnEnable = true;
    private PlayerInputMap _genInputMap;
    private PlayerInputMap.PlayerActions _playerActions;
    private PlayerInputMap.CardControlsActions _cardActions;
    public PlayerInputMap.CardControlsActions CardActions => _cardActions;
    private EventSystem _uiEventSystem;
    /// <summary>
    /// Gets called everytime the Move Input is performed (every frame it's pressed) or the frame it is canceled.
    /// </summary>
    public static event Action<Vector2> OnMoveInteracted;
    public bool WasSprintPressedThisFrame { get { return this._playerActions.Sprint.WasPressedThisFrame(); } }
    public bool WasMovePressedThisFrame { get { return this._playerActions.Move.WasPressedThisFrame(); } }
    public bool WasEscapePressedThisFrame { get { return this._playerActions.Escape.WasPressedThisFrame(); } }

    /// <summary>
    /// References to InputActions for each action in the revolver chamber, movement and shift. 
    /// </summary>
    public InputAction Move
    {
        get
        {
            return _playerActions.Move;
        }
    }
    public InputAction Sprint
    {
        get
        {
            return _playerActions.Sprint;
        }
    }
    public InputAction Escape
    {
        get
        {
            return _playerActions.Escape;
        }
    }
    void Awake()
    {
        _genInputMap = new PlayerInputMap();
        _playerActions = _genInputMap.Player;
        this.GetComponentInScene<EventSystem>(false, out _uiEventSystem);
        if (_uiEventSystem != null)
        {
            if (!_uiEventSystem.isActiveAndEnabled)
            {
                _uiEventSystem.enabled = true;
            }
        }
    }


    void OnEnable()
    {
        _playerActions.Move.performed += DetectedMovement;
        _playerActions.Move.canceled += DetectedMovement;
        ActivatePlayerInputs(_activatePlayerInputsOnEnable);
        ActivateUIInputs(_activateUIInputsOnEnable);
    }
    void OnDisable()
    {
        _playerActions.Move.performed -= DetectedMovement;
        _playerActions.Move.canceled -= DetectedMovement;
        ActivatePlayerInputs(false);
        ActivateUIInputs(false);
    }
    public void ActivatePlayerInputs(bool activate)
    {
        if (activate)
        {
            _playerActions.Enable();
        }
        else
        {
            _playerActions.Disable();
        }
    }
    public void ActivateUIInputs(bool activate)
    {
        if (_uiEventSystem != null)
        {
            _uiEventSystem.enabled = activate;
            return;
        }
        Debug.LogWarning("Input Manager doesn't contain an assigned Event System.");
    }
    public void DebugPrinter(string message)
    {
        Debug.Log(message);
    }
    private void DetectedMovement(InputAction.CallbackContext callbackContext)
    {
        OnMoveInteracted?.Invoke(callbackContext.ReadValue<Vector2>());
    }
}
