using System.Collections.Generic;
using System.Linq;
using CardSystem;
using UnityEngine;

public class RevolverManagerUI : MonoBehaviour
{
    [SerializeField]
    private List<BulletManagerUI> _bullets;
    [SerializeField]
    private GameObject _reloadIndicator;

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
    }

    // Remove Start method that was disabling bullets after card initialization

    public void LoadCard(BulletDirection direction, Card action)
    {
        if (action == null)
        {
            Debug.LogWarning("Attempted to load null card");
            return;
        }

        BulletManagerUI bullet = _bullets.Where(bullet => bullet.Direction == direction).
        FirstOrDefault();

        if (bullet == null)
        {
            Debug.LogWarning($"No bullet UI found for direction {direction}");
            return;
        }

        bullet.gameObject.SetActive(true);
        bullet.ActivateBullet(action);
    }

    public void DiscardCard(BulletDirection direction)
    {
        BulletManagerUI bullet = _bullets.Where(bullet => bullet.Direction == direction).
        FirstOrDefault();

        if (bullet == null)
        {
            return;
        }

        bullet.gameObject.SetActive(false);
        bullet.DisableBullet();
    }
}
