using UnityEngine;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    private void Start()
    {
        // Load saved values into UI
        volumeSlider.value = PersistentSettings.GlobalVolume;
        fullscreenToggle.isOn = PersistentSettings.IsFullScreen;

        // Save values when changed
        volumeSlider.onValueChanged.AddListener(val => PersistentSettings.GlobalVolume = val);
        fullscreenToggle.onValueChanged.AddListener(val => PersistentSettings.IsFullScreen = val);
    }
}

