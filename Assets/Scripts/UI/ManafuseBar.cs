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
    [Header("Dependencies")]
    [SerializeField]
    private Slider greenSlider;
    [SerializeField]
    private Slider redSlider;
    [SerializeField]
    private Pool dividerPool;

    public float MaxSliderValue { get { return greenSlider.maxValue; } }
    public float TicksPerUnit { get { return MaxSliderValue / unitDividers; } }
    void Start()
    {
        ActivateDividers();
        redSlider.maxValue = MaxSliderValue;
    }
    void Update()
    {
        if (redSlider.value > greenSlider.value)
        {
            redSlider.value -= Time.deltaTime * (TicksPerUnit * redManaDrainTime);
        }
    }
    private void ActivateDividers()
    {
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
}
