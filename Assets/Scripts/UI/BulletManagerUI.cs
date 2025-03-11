
using CardSystem;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BulletManagerUI : MonoBehaviour
{

    [SerializeField]
    private BulletDirection _direction;
    [SerializeField]
    private Image _bulletImage;
    private Pool _energyNodesPool;
    private RectTransform _imageRT;
    public BulletDirection Direction => _direction;
    void Awake()
    {
        _energyNodesPool = GetComponentInChildren<Pool>(true);
        _imageRT = GetComponent<RectTransform>();
    }
    public void ActivateBullet(Card cardToSet)
    {
        _bulletImage.enabled = true;
        _bulletImage.sprite = cardToSet.cardIcon;
        _bulletImage.SetNativeSize();
        SetEnergyNodes(cardToSet.cardCost);
    }
    public void ActivateBullet(int ManaCost, Sprite bulletSprite)
    {
        _bulletImage.enabled = true;
        _bulletImage.sprite = bulletSprite;
        _bulletImage.SetNativeSize();
        SetEnergyNodes(ManaCost);
    }
    public void DisableBullet()
    {
        _energyNodesPool.SetAllObjectsInactive();
        _bulletImage.enabled = false;
    }

    private void SetEnergyNodes(int noNodes)
    {
        //Do somethin.
        float step = 360f / (float)noNodes;
        //float radius = noNodes / 8;
        float bulletImageRadius = _imageRT.sizeDelta.y / 2;
        for (int i = 0; i < noNodes; i++)
        {
            GameObject activeEnergyNode = _energyNodesPool.GetInactiveObject();
            activeEnergyNode.SetActive(true);
            activeEnergyNode.transform.localRotation = Quaternion.Euler(0, 0, i * step);
            activeEnergyNode.transform.localPosition = activeEnergyNode.transform.up * bulletImageRadius;
        }
    }

}
