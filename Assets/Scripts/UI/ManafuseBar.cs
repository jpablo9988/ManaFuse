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
            redSlider.value -= Time.deltaTime * (TicksPerUnit * redManaDrainTime);
        }

    }
    void OnEnable()
    {
        redSlider.onValueChanged.AddListener(delegate { CheckRecovery(); });
    }
    void OnDisable()
    {
        redSlider.onValueChanged.RemoveAllListeners();
    }
    private void CheckRecovery()
    {
        if (!callsUpdate && redSlider.value > redSlider.minValue)
        {
            callsUpdate = true;
            NoManaLeft?.Invoke(false);
        }
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
        redSlider.maxValue = MaxSliderValue;
        if (resetManaValues)
        {
            greenSlider.value = MaxSliderValue;
            redSlider.value = MaxSliderValue;
        }
    }
}
