using UnityEngine;
using UnityEngine.UI;

public class LowHealthBorderUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image warningImage;
    [SerializeField] private PlayerManager playerManager;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float healthThreshold = 0.25f;
    [SerializeField] private float fadeSpeed = 3f;

    private bool isLowHealth = false;
    private int maxManaUnits = 0;
    //private float alphaOffset = 0f; // controls starting alpha for fade-in

    private void Awake()
    {
        if (playerManager == null)
            playerManager = FindFirstObjectByType<PlayerManager>();

        if (warningImage == null)
        {
            Debug.LogError("LowHealthWarning: No warning image assigned!");
            enabled = false;
            return;
        }

        warningImage.enabled = false;
        SetImageAlpha(0f);

        if (playerManager != null)
        {
            maxManaUnits = GetMaxManaUnits();
        }
    }

    private void Update()
    {
        if (playerManager == null) return;

        int currentMana = playerManager.CurrentManaUnits;
        if (maxManaUnits == 0)
            maxManaUnits = GetMaxManaUnits();

        bool shouldBeLowHealth = currentMana <= maxManaUnits * healthThreshold;

        if (shouldBeLowHealth && !isLowHealth)
        {
            // Player just went below threshold → start fade-in
            isLowHealth = true;
            warningImage.enabled = true;
            //alphaOffset = 0f; // start at 0 opacity
        }
        else if (!shouldBeLowHealth && isLowHealth)
        {
            isLowHealth = false;
            warningImage.enabled = false;
        }

        if (isLowHealth)
        {
            // Pulsing fade, starting from alphaOffset
            float alpha = (Mathf.Sin(Time.time * fadeSpeed) + 1f) * 0.5f; // 0–1
            alpha = Mathf.Lerp(0f, 1f, alpha); // ensure fade starts from 0
            SetImageAlpha(alpha);
        }
    }

    private int GetMaxManaUnits()
    {
        var field = typeof(PlayerManager).GetField("_manaUnits", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            return (int)field.GetValue(playerManager);
        }

        Debug.LogWarning("LowHealthWarning: Could not read max mana units; defaulting to 15.");
        return 15;
    }

    private void SetImageAlpha(float alpha)
    {
        if (warningImage == null) return;

        Color c = warningImage.color;
        c.a = alpha;
        warningImage.color = c;
    }
}
