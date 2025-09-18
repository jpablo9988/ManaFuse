using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardSystem;
using UnityEngine;
using UnityEngine.UI;

public class RevolverManagerUI : MonoBehaviour
{
    [SerializeField]
    private List<BulletManagerUI> _bullets;
    [SerializeField]
    private GameObject _reloadIndicator;
    [SerializeField]
    private Image _revolverImage; // Main revolver UI image to glow
    [SerializeField]
    private float _glowIntensity = 2f;
    [SerializeField]
    private float _glowSpeed = 5f;
    [SerializeField]
    private Color _glowColor = Color.blue; // Blue glow color
    [SerializeField]
    private Material _glowMaterial; // Material with glow effect
    
    private Color _originalColor;
    private bool _isGlowing = false;
    private Coroutine _glowCoroutine;
    private Material _originalMaterial;

    public bool ShowReloadIndicator
    {
        get
        {
            return _reloadIndicator.activeSelf;
        }
        set
        {
            _reloadIndicator.SetActive(value);
        }
    }

    void Awake()
    {
        _bullets = new();
        _bullets = GetComponentsInChildren<BulletManagerUI>().ToList();

        // Ensure bullets are initially invisible until cards are loaded
        foreach (BulletManagerUI bullet in _bullets)
        {
            bullet.gameObject.SetActive(false);
        }

        // Store original color and material for the revolver image
        if (_revolverImage != null)
        {
            _originalColor = _revolverImage.color;
            _originalMaterial = _revolverImage.material;
        }
    }

    // Remove Start method that was disabling bullets after card initialization

    public void LoadCard(BulletDirection direction, Card action)
    {
        if (!action)
        {
            Debug.LogWarning("Attempted to load null card");
            return;
        }

        BulletManagerUI bullet = _bullets.
            FirstOrDefault(bullet => bullet.Direction == direction);

        if (!bullet)
        {
            Debug.LogWarning($"No bullet UI found for direction {direction}");
            return;
        }

        bullet.gameObject.SetActive(true);
        bullet.ActivateBullet(action);
    }

    public void DiscardCard(BulletDirection direction)
    {
        BulletManagerUI bullet = _bullets.
            FirstOrDefault(bullet => bullet.Direction == direction);

        if (!bullet)
        {
            return;
        }

        bullet.gameObject.SetActive(false);
        bullet.DisableBullet();
    }

    /// <summary>
    /// Starts the glow effect on the revolver image when reload modifier is held
    /// </summary>
    public void StartGlow()
    {
        if (_revolverImage == null || _isGlowing) return;
        
        _isGlowing = true;
        
        // Apply glow material if available
        if (_glowMaterial != null)
        {
            _revolverImage.material = _glowMaterial;
        }
        
        if (_glowCoroutine != null)
        {
            StopCoroutine(_glowCoroutine);
        }
        _glowCoroutine = StartCoroutine(GlowEffect());
    }

    /// <summary>
    /// Stops the glow effect and returns to original color
    /// </summary>
    public void StopGlow()
    {
        if (!_isGlowing) return;
        
        _isGlowing = false;
        if (_glowCoroutine != null)
        {
            StopCoroutine(_glowCoroutine);
            _glowCoroutine = null;
        }
        
        if (_revolverImage != null)
        {
            _revolverImage.color = _originalColor;
            _revolverImage.material = _originalMaterial;
        }
    }

    /// <summary>
    /// Coroutine that creates a pulsing blue glow effect that never fully returns to original
    /// </summary>
    private IEnumerator GlowEffect()
    {
        while (_isGlowing && _revolverImage != null)
        {
            float time = Time.time * _glowSpeed;
            float glowValue = (Mathf.Sin(time) + 1f) * 0.5f; // 0 to 1
            
            // Create a base blue tint that's always present (minimum 30% blue)
            float minBlueTint = 0.3f;
            float maxBlueTint = minBlueTint + (glowValue * (_glowIntensity - minBlueTint));
            
            // Interpolate between original color and blue, but never go below minBlueTint
            Color glowColor = Color.Lerp(_originalColor, _glowColor, maxBlueTint);
            _revolverImage.color = glowColor;
            
            // If using a glow material, we can also adjust its properties
            if (_glowMaterial != null)
            {
                // Set glow intensity in the material (if it has a _GlowIntensity property)
                if (_glowMaterial.HasProperty("_GlowIntensity"))
                {
                    _glowMaterial.SetFloat("_GlowIntensity", maxBlueTint);
                }
                // Set glow color in the material (if it has a _GlowColor property)
                if (_glowMaterial.HasProperty("_GlowColor"))
                {
                    _glowMaterial.SetColor("_GlowColor", _glowColor);
                }
            }
            
            yield return null;
        }
    }
}
