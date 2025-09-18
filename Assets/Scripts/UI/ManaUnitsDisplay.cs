using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the current mana units as a 2-digit number using sprite images (0-9)
/// </summary>
public class ManaUnitsDisplay : MonoBehaviour
{
    [Header("Number Sprites")]
    [SerializeField]
    [Tooltip("Array of sprites for digits 0-9. Index 0 = sprite for '0', Index 1 = sprite for '1', etc.")]
    private Sprite[] _numberSprites = new Sprite[10];

    [Header("UI References")]
    [SerializeField]
    [Tooltip("Image component for the tens digit (left digit)")]
    private Image _tensDigitImage;

    [SerializeField]
    [Tooltip("Image component for the ones digit (right digit)")]
    private Image _onesDigitImage;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("Whether to show leading zeros (e.g., show '05' instead of just '5')")]
    private bool _showLeadingZero = true;

    [SerializeField]
    [Tooltip("Whether to hide the display when mana units is 0")]
    private bool _hideOnZero = false;

    private PlayerManager _playerManager;
    private int _lastDisplayedValue = -1; // Cache the last displayed value to prevent unnecessary updates

    void Awake()
    {
        // Find PlayerManager in the scene
        _playerManager = FindFirstObjectByType<PlayerManager>();
        if (_playerManager == null)
        {
            Debug.LogError("ManaUnitsDisplay: PlayerManager not found in scene!");
            enabled = false; // Disable the component to prevent further errors
            return;
        }

        // Validate number sprites array
        if (_numberSprites == null || _numberSprites.Length != 10)
        {
            Debug.LogError("ManaUnitsDisplay: Number sprites array must contain exactly 10 sprites (0-9)!");
            enabled = false;
            return;
        }

        // Check for missing sprites
        for (int i = 0; i < _numberSprites.Length; i++)
        {
            if (_numberSprites[i] == null)
            {
                Debug.LogError($"ManaUnitsDisplay: Number sprite for digit {i} is missing!");
                enabled = false;
                return;
            }
        }

        // Validate UI image references
        if (_tensDigitImage == null || _onesDigitImage == null)
        {
            Debug.LogError("ManaUnitsDisplay: Tens digit image and/or ones digit image references are missing!");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        // Initial display update
        UpdateDisplay();
    }

    void Update()
    {
        // Update display only when the value changes (optimized)
        if (_playerManager != null)
        {
            int currentValue = GetCurrentManaUnits();
            if (currentValue != _lastDisplayedValue)
            {
                UpdateDisplay();
                _lastDisplayedValue = currentValue;
            }
        }
    }

    /// <summary>
    /// Updates the display to show the current mana units
    /// </summary>
    public void UpdateDisplay()
    {
        if (_playerManager == null) return;

        // Get current mana units from PlayerManager
        int currentManaUnits = GetCurrentManaUnits();

        // Handle hiding on zero
        if (_hideOnZero && currentManaUnits == 0)
        {
            _tensDigitImage.gameObject.SetActive(false);
            _onesDigitImage.gameObject.SetActive(false);
            return;
        }

        // Show images if they were hidden
        _tensDigitImage.gameObject.SetActive(true);
        _onesDigitImage.gameObject.SetActive(true);

        // Calculate tens and ones digits
        int tensDigit = currentManaUnits / 10;
        int onesDigit = currentManaUnits % 10;

        // Clear any existing sprites first to prevent stacking
        _tensDigitImage.sprite = null;
        _onesDigitImage.sprite = null;

        // Handle leading zero display
        if (!_showLeadingZero && tensDigit == 0)
        {
            // Hide tens digit and show only ones digit
            _tensDigitImage.gameObject.SetActive(false);
            _onesDigitImage.sprite = _numberSprites[onesDigit];
        }
        else
        {
            // Show both digits
            _tensDigitImage.gameObject.SetActive(true);
            _tensDigitImage.sprite = _numberSprites[tensDigit];
            _onesDigitImage.sprite = _numberSprites[onesDigit];
        }
    }

    /// <summary>
    /// Gets the current mana units from PlayerManager
    /// </summary>
    private int GetCurrentManaUnits()
    {
        if (_playerManager != null)
        {
            return _playerManager.CurrentManaUnits;
        }

        return 0;
    }

    /// <summary>
    /// Manually set the display value (for testing or direct control)
    /// </summary>
    /// <param name="value">The value to display (0-99)</param>
    public void SetDisplayValue(int value)
    {
        value = Mathf.Clamp(value, 0, 99);

        // Update cache to prevent unnecessary updates
        _lastDisplayedValue = value;

        if (_hideOnZero && value == 0)
        {
            _tensDigitImage.gameObject.SetActive(false);
            _onesDigitImage.gameObject.SetActive(false);
            return;
        }

        _tensDigitImage.gameObject.SetActive(true);
        _onesDigitImage.gameObject.SetActive(true);

        int tensDigit = value / 10;
        int onesDigit = value % 10;

        // Clear any existing sprites first to prevent stacking
        _tensDigitImage.sprite = null;
        _onesDigitImage.sprite = null;

        if (!_showLeadingZero && tensDigit == 0)
        {
            _tensDigitImage.gameObject.SetActive(false);
            _onesDigitImage.sprite = _numberSprites[onesDigit];
        }
        else
        {
            _tensDigitImage.gameObject.SetActive(true);
            _tensDigitImage.sprite = _numberSprites[tensDigit];
            _onesDigitImage.sprite = _numberSprites[onesDigit];
        }
    }
}
