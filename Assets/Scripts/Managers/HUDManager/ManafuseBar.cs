using System;
using UnityEngine;
using UnityEngine.UI;

public class ManafuseBar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int unitDividers = 10;
    [Tooltip("How much will it take for the red mana to drain per one unit. ")]
    [SerializeField]
    private float redManaDrainTime = 0.5f;
    [SerializeField]
    private float maxTicks = 100f;
    [Header("Dependencies")]
    [SerializeField]
    private Slider greenSlider;
    [SerializeField]
    private Slider redSlider;
    [SerializeField]
    private Pool dividerPool;
    
    [Header("Edge Sprite")]
    [SerializeField]
    [Tooltip("Optional sprite that follows the green slider's edge")]
    public RectTransform edgeSprite;
    [SerializeField]
    [Tooltip("Offset from the slider edge (in pixels)")]
    public float edgeSpriteOffset = 0f;

    public float MaxSliderValue { get { return greenSlider.maxValue; } }
    public float TicksPerUnit { get { return MaxSliderValue / unitDividers; } }
    public static event Action<bool> NoManaLeft;
    private bool callsUpdate = true;


    void Update()
    {
        if (!callsUpdate) return;
        if (redSlider.value <= redSlider.minValue)
        {
            NoManaLeft?.Invoke(true);
            callsUpdate = false;
            return;
        }
        if (redSlider.value > greenSlider.value)
        {
            redSlider.value -= Time.deltaTime * (TicksPerUnit * (1 / redManaDrainTime));
        }

    }
    void OnEnable()
    {
        redSlider.onValueChanged.AddListener(delegate { CheckRecovery(); });
        greenSlider.onValueChanged.AddListener(delegate { UpdateEdgeSpritePosition(); });
    }
    void OnDisable()
    {
        redSlider.onValueChanged.RemoveAllListeners();
        greenSlider.onValueChanged.RemoveAllListeners();
    }
    private void CheckRecovery()
    {
        if (!callsUpdate && redSlider.value > redSlider.minValue)
        {
            callsUpdate = true;
            NoManaLeft?.Invoke(false);
        }
    }
    
    /// <summary>
    /// Updates the position of the edge sprite to follow the green slider's current value.
    /// The sprite will only follow the green slider, not the red one.
    /// </summary>
    private void UpdateEdgeSpritePosition()
    {
        if (edgeSprite == null || greenSlider == null) return;
        
        // Calculate the normalized position of the green slider (0 to 1)
        float normalizedPosition = greenSlider.value / greenSlider.maxValue;
        
        // Get the slider's fill area (the actual visual bar)
        RectTransform sliderFillArea = greenSlider.fillRect;
        if (sliderFillArea == null) return;
        
        // Calculate the position along the slider's width
        float sliderWidth = sliderFillArea.rect.width;
        float targetX = (normalizedPosition * sliderWidth) + edgeSpriteOffset;
        
        // Set the edge sprite's position
        Vector3 newPosition = edgeSprite.anchoredPosition;
        newPosition.x = targetX;
        edgeSprite.anchoredPosition = newPosition;
    }
    private void ActivateDividers()
    {
        dividerPool.SetAllObjectsInactive(); //Refresh Mana.
        for (int i = 0; i < unitDividers; i++)
        {
            GameObject divider = dividerPool.GetInactiveObject();
            divider.SetActive(true);
            if (i == unitDividers - 1)
            {
                divider.GetComponent<Image>().enabled = false;
            }
        }
    }

    public void ChangeByTick(float amount, bool includeRedSlider)
    {
        greenSlider.value += amount;
        greenSlider.value = Mathf.Clamp(greenSlider.value, 0, MaxSliderValue);
        if (includeRedSlider)
        {
            redSlider.value += amount;
            redSlider.value = Mathf.Clamp(redSlider.value, 0, MaxSliderValue);
        }
        if (greenSlider.value > redSlider.value)
        {
            redSlider.value = greenSlider.value;
        }
    }
    public void ChangeByUnit(int noUnits, bool includeRedSlider)
    {
        float valueToChange = TicksPerUnit * noUnits;
        ChangeByTick(valueToChange, includeRedSlider);
    }
    public void SetManaUnits(int units, int ticks = 100, bool resetManaValues = true)
    {
        unitDividers = units;
        if (maxTicks != ticks)
            maxTicks = (float)ticks;
        ActivateDividers();
        greenSlider.maxValue = maxTicks;
        redSlider.maxValue = maxTicks;
        if (resetManaValues)
        {
            greenSlider.value = MaxSliderValue;
            redSlider.value = MaxSliderValue;
        }
        
        // Update edge sprite position after setting up the slider
        UpdateEdgeSpritePosition();
    }
    
    /// <summary>
    /// Public method to manually update the edge sprite position.
    /// Useful for testing or external control.
    /// </summary>
    public void RefreshEdgeSpritePosition()
    {
        UpdateEdgeSpritePosition();
    }
}
