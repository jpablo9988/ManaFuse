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
        
        if (_bulletImage == null)
        {
            _bulletImage = GetComponent<Image>();
        }
        
        // Ensure bullet image is initially disabled
        if (_bulletImage != null)
        {
            _bulletImage.enabled = false;
        }
    }
    
    public void ActivateBullet(Card cardToSet)
    {
        if (cardToSet == null)
        {
            Debug.LogWarning($"Tried to activate bullet with null card in {_direction}");
            return;
        }
        
        if (_bulletImage == null)
        {
            Debug.LogError($"Bullet image is null in {_direction}");
            return;
        }
        
        _bulletImage.enabled = true;
        _bulletImage.sprite = cardToSet.cardIcon;
        _bulletImage.SetNativeSize();
        SetEnergyNodes(cardToSet.cardCost);
        
        Debug.Log($"Bullet {_direction} activated with {cardToSet.cardName}");
    }
    
    public void ActivateBullet(int ManaCost, Sprite bulletSprite)
    {
        if (_bulletImage == null)
        {
            Debug.LogError($"Bullet image is null in {_direction}");
            return;
        }
        
        _bulletImage.enabled = true;
        _bulletImage.sprite = bulletSprite;
        _bulletImage.SetNativeSize();
        SetEnergyNodes(ManaCost);
    }
    
    public void DisableBullet()
    {
        if (_energyNodesPool != null)
        {
            _energyNodesPool.SetAllObjectsInactive();
        }
        
        if (_bulletImage != null)
        {
            _bulletImage.enabled = false;
        }
    }

    private void SetEnergyNodes(int noNodes)
    {
        if (_energyNodesPool == null)
        {
            Debug.LogError($"Energy nodes pool is null in {_direction}");
            return;
        }
        
        // Clear existing nodes
        _energyNodesPool.SetAllObjectsInactive();
        
        float step = 360f / (float)noNodes;
        float bulletImageRadius = _imageRT.sizeDelta.y / 2;
        for (int i = 0; i < noNodes; i++)
        {
            GameObject activeEnergyNode = _energyNodesPool.GetInactiveObject();
            if (activeEnergyNode == null)
            {
                Debug.LogWarning($"Not enough energy nodes in pool for {_direction}");
                continue;
            }
            
            activeEnergyNode.SetActive(true);
            activeEnergyNode.transform.localRotation = Quaternion.Euler(0, 0, i * step);
            activeEnergyNode.transform.localPosition = activeEnergyNode.transform.up * bulletImageRadius;
        }
    }
}
