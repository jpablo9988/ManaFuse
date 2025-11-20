using System;
using TMPro;
using UnityEngine;
using SaveSystem;

[RequireComponent(typeof(PersistentId))]
public class PlayerManager : MonoBehaviour, ISaveable
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

    private PersistentId _persistentId;

    public bool IsTimerActive { get => _timer.IsTicking; set => _timer.IsTicking = value; }
    public PlayerMovement PlayerMovementManager => _movement;
    public static event Action OnDeathPlayer;

    /// <summary>
    /// Gets the current number of mana units
    /// </summary>
    public int CurrentManaUnits
    {
        get
        {
            if (_bar && _bar.gameObject.activeInHierarchy)
            {
                try
                {
                    // Get the green slider from the ManafuseBar
                    var greenSlider = _bar.GetComponentInChildren<UnityEngine.UI.Slider>();
                    if (greenSlider != null)
                    {
                        // Calculate current units based on slider value
                        float currentSliderValue = greenSlider.value;
                        float ticksPerUnit = _bar.TicksPerUnit;
                        return Mathf.FloorToInt(currentSliderValue / ticksPerUnit);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"Error calculating current mana units: {e.Message}");
                }
            }
            return _manaUnits;
        }
    }

    void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _persistentId = GetComponent<PersistentId>();
    }
    void Start()
    {
        if (!_bar)
        {
            _bar = this.GetComponentInScene(false, out _bar);
            if (!_bar)
            {
#if UNITY_EDITOR
                Debug.LogError("Failed to find ManafuseBar component");
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }
        }
        if (!_timer)
        {
            _timer = this.GetComponentInScene(false, out _timer);
            if (!_timer)
            {
#if UNITY_EDITOR
                Debug.LogError("Failed to find Timer component");
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }
        }
        if (!_movement)
        {
            _movement = this.GetComponentInScene(false, out _movement);
            if (!_movement)
            {
#if UNITY_EDITOR
                Debug.LogError("Failed to find Movement component");
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }
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

        // Register with save system
        SaveGameManager.Instance.Register(this);
    }
    void OnDisable()
    {
        ManafuseBar.NoManaLeft -= IsGameOver;

        // Unregister from save system
        SaveGameManager.Instance.Unregister(this);
    }
    public void SetManaUnits(int newUnits, int newTicks = 0)
    {
        if (!_bar.gameObject.activeSelf) return;
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
        if (_bar.gameObject.activeSelf)
        {
            _bar.ChangeByTick(unit, includeRedSlider);
        }
    }

    private void ReduceManaByTickUnit(float unit)
    {
        if (_bar.gameObject.activeSelf)
        {
            _bar.ChangeByTick(-unit, true);
        }
    }
    public void ChangeMana(int unitAmount, bool includeRedSlider)
    {
        if (_bar.gameObject.activeSelf)
        {
            _bar.ChangeByUnit(unitAmount, includeRedSlider);
        }
    }
    private void IsGameOver(bool gameOver)
    {
        if (gameOver)
        {
            _movement.StopAllMovement();
            OnDeathPlayer?.Invoke();
        }
        else
        {
            print("You have been resusitated");
        }
        GameContext.Instance.InputManager.ActivatePlayerInputs = !gameOver;
        GameContext.Instance.InputManager.ActivateCardInputs = !gameOver;


    }

    // ISaveable Implementation
    public string SaveId => _persistentId ? _persistentId.Id : string.Empty;

    public object CaptureState()
    {
        float greenValue = 0f;
        float redValue = 0f;

        // Get current slider values from the bar
        if (_bar && _bar.gameObject.activeInHierarchy)
        {
            var sliders = _bar.GetComponentsInChildren<UnityEngine.UI.Slider>();
            if (sliders.Length >= 2)
            {
                greenValue = sliders[0].value; // First slider is green
                redValue = sliders[1].value;   // Second slider is red
            }
        }

        return new PlayerSaveData
        {
            manaUnits = _manaUnits,
            manaBarTicks = _manaBarTicks,
            greenSliderValue = greenValue,
            redSliderValue = redValue,
            positionX = transform.position.x,
            positionY = transform.position.y,
            positionZ = transform.position.z,
            roundedAngle = _movement ? _movement.RoundedAngle : 180f
        };
    }

    public void RestoreState(object state)
    {
        if (state is PlayerSaveData data)
        {
            // Restore position
            transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);

            // Restore rotation through PlayerMovement
            if (_movement != null)
            {
                _movement.SetRotation(data.roundedAngle);
            }

            // Restore mana bar configuration
            if (_bar && _bar.gameObject.activeInHierarchy)
            {
                // Set up the bar with saved units and ticks (but don't reset values)
                _bar.SetManaUnits(data.manaUnits, data.manaBarTicks, false);

                // Restore the actual slider values
                var sliders = _bar.GetComponentsInChildren<UnityEngine.UI.Slider>();
                if (sliders.Length >= 2)
                {
                    sliders[0].value = data.greenSliderValue; // Green slider
                    sliders[1].value = data.redSliderValue;   // Red slider
                }
            }

            Debug.Log($"Player state restored: Mana={data.greenSliderValue}/{data.manaBarTicks}, Position={transform.position}, Rotation={data.roundedAngle}");
        }
    }

    [Serializable]
    private class PlayerSaveData
    {
        public int manaUnits;
        public int manaBarTicks;
        public float greenSliderValue;
        public float redSliderValue;
        public float positionX;
        public float positionY;
        public float positionZ;
        public float roundedAngle;
    }
}
