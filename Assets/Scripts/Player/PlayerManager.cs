using System;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Start Settings")]
    [SerializeField]
    private bool _startTimerTicking = true;
    [Header("Mana")]
    [SerializeField]
    [Tooltip("The amount of mana whole units the bar will have. ")]
    private int _manaUnits;
    [SerializeField]
    [Tooltip("The amount of seconds the bar has until depletion. ")]
    private int _manaBarTicks;
    [Header("Dependencies")]
    [SerializeField]
    private ManafuseBar _bar;
    [SerializeField]
    private Timer _timer;
    [SerializeField]
    private PlayerMovement _movement;

    public bool IsTimerActive { get => _timer.IsTicking; set => _timer.IsTicking = value; }
    public PlayerMovement PlayerMovementManager => _movement;

    void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        if (_bar == null)
        {
            _bar = this.GetComponentInScene(false, out _bar);
        }
        if (_timer == null)
        {
            _timer = this.GetComponentInScene(false, out _timer);
        }
        if (_movement == null)
        {
            _movement = this.GetComponentInScene(false, out _movement);
        }
        _bar.SetManaUnits(_manaUnits, _manaBarTicks);

    }
    void OnEnable()
    {
        if (_timer != null)
        {
            _timer.InitializeTimer(_startTimerTicking, ReduceManaByTickUnit);
        }
        ManafuseBar.NoManaLeft += IsGameOver;
    }
    void OnDisable()
    {
        ManafuseBar.NoManaLeft -= IsGameOver;
    }
    public void SetManaUnits(int newUnits, int newTicks = 0)
    {
        _manaUnits = newUnits;
        if (newTicks > 0)
        {
            _bar.SetManaUnits(_manaUnits, newTicks, false);
        }
        else
        {
            _bar.SetManaUnits(_manaUnits, (int)_bar.MaxSliderValue, false);
        }
    }
    public void ChangeManaByTickUnit(float unit, bool includeRedSlider = true)
    {
        _bar.ChangeByTick(unit, includeRedSlider);
    }

    private void ReduceManaByTickUnit(float unit)
    {
        _bar.ChangeByTick(-unit, true);
    }
    public void ChangeMana(int unitAmount, bool includeRedSlider)
    {
        _bar.ChangeByUnit(unitAmount, includeRedSlider);
    }
    private void IsGameOver(bool gameOver)
    {
        if (gameOver)
        {
            Debug.Log("Game Over");
        }
        else
        {
            Debug.Log("You have been resusitated");
        }
        GameContext.Instance.InputManager.ActivatePlayerInputs = !gameOver;
        GameContext.Instance.InputManager.ActivateCardInputs = !gameOver;


    }
}
