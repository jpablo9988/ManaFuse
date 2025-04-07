using UnityEngine;

public class PersistentSettings
{
    private const string VolumeKey = "GlobalVolume";
    private const string FullscreenKey = "IsFullScreen";

    // Default values
    private const float DefaultVolume = 1.0f;
    private const bool DefaultFullscreen = true;

    // Volume from 0.0 - 1.0
    public static float GlobalVolume
    {
        get => PlayerPrefs.GetFloat(VolumeKey, DefaultVolume);
        set
        {
            PlayerPrefs.SetFloat(VolumeKey, Mathf.Clamp01(value));
            PlayerPrefs.Save();
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
        }
    }

    // Call on app start if needed
    public static void ApplySettings()
    {
        AudioListener.volume = GlobalVolume;
        Screen.fullScreen = IsFullScreen;
    }
}
