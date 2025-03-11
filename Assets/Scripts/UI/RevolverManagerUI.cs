using System.Collections.Generic;
using System.Linq;
using CardSystem;
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

    public void LoadCard(BulletDirection direction, Card action)
    {
        BulletManagerUI bullet = _bullets.Where(bullet => bullet.Direction == direction).
        FirstOrDefault();
        bullet.gameObject.SetActive(true);
        bullet.ActivateBullet(action);
    }
    public void DiscardCard(BulletDirection direction)
    {
        BulletManagerUI bullet = _bullets.Where(bullet => bullet.Direction == direction).
        FirstOrDefault();
        bullet.gameObject.SetActive(false);
        bullet.DisableBullet();
    }
}
