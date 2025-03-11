using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RevolverManagerUI : MonoBehaviour
{
    [SerializeField]
    private List<BulletManagerUI> _bullets;
    void Awake()
    {
        _bullets = new();
        _bullets = GetComponentsInChildren<BulletManagerUI>().ToList();
    }
    void Start()
    {
        foreach (BulletManagerUI bullet in _bullets)
        {
            bullet.gameObject.SetActive(false);
        }
    }

    public void ReloadBullet(BulletDirection direction, BulletAction action)
    {
        BulletManagerUI bullet = _bullets.Where(bullet => bullet.Direction == direction).
        FirstOrDefault();
        bullet.gameObject.SetActive(true);
        bullet.ActivateBullet(action);
    }
    public void RemoveBullet(BulletDirection direction)
    {
        BulletManagerUI bullet = _bullets.Where(bullet => bullet.Direction == direction).
        FirstOrDefault();
        bullet.gameObject.SetActive(false);
        bullet.DisableBullet();
    }
}
