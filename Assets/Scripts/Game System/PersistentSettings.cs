using UnityEngine;

public static class PersistentSettings
{
    private const string VolumeKey = "GlobalVolume";
    private const string FullscreenKey = "IsFullScreen";

    private const float DefaultVolume = 1.0f;
    private const bool DefaultFullscreen = true;

    // Volume value between 0 and 1
    public static float GlobalVolume
    {
        get => PlayerPrefs.GetFloat(VolumeKey, DefaultVolume);
        set
        {
            PlayerPrefs.SetFloat(VolumeKey, Mathf.Clamp01(value));
            PlayerPrefs.Save();
            AudioListener.volume = value; // Apply immediately
        }
    }

    // Fullscreen toggle
    public static bool IsFullScreen
    {
        get => PlayerPrefs.GetInt(FullscreenKey, DefaultFullscreen ? 1 : 0) == 1;
        set
        {
            PlayerPrefs.SetInt(FullscreenKey, value ? 1 : 0);
            PlayerPrefs.Save();
            Screen.fullScreen = value; // Apply immediately
        }
    }

    // Apply saved settings at application start
    public static void ApplySettings()
    {
        AudioListener.volume = GlobalVolume;
        Screen.fullScreen = IsFullScreen;
    }

    // Optional: reset all settings to default values
    public static void ResetToDefault()
    {
        GlobalVolume = DefaultVolume;
        IsFullScreen = DefaultFullscreen;
    }
}
