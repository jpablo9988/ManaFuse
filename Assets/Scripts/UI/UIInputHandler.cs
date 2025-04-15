using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputHandler : MonoBehaviour
{
    private PlayerInputMap _inputActions;
    private PlayerInputMap.UIActions _uiActions;
    private InputAction _pauseAction;
    void Awake()
    {
        _inputActions = new PlayerInputMap();
        _uiActions = _inputActions.UI;
        _pauseAction = _uiActions.Pause;
    }
    void OnEnable()
    {
        _inputActions.Enable();
        _pauseAction.started += ctx => Pause(); //Should only trigger once. (When _sprintAction is pressed)
    }
    void OnDisable()
    {
        _inputActions.Disable();
        _pauseAction.started -= ctx => Pause(); //Should only trigger once. (When _sprintAction is pressed)
    }

    public void Pause()
    {
        GameContext.Instance.PauseHandler.ChangePauseState();
    }
}
