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
    [SerializeField]
    private Image _numberImage; // New UI Image for displaying the number sprite
    [SerializeField]
    private Sprite[] _numberSprites; // Array of sprites for digits 0-9
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

        // Ensure bullet and number images are initially disabled
        if (_bulletImage != null)
        {
            _bulletImage.enabled = false;
        }

        if (_numberImage != null)
        {
            _numberImage.enabled = false;
        }
    }

    public void ActivateBullet(Card cardToSet)
    {
        if (!cardToSet)
        {
            print($"Tried to activate bullet with null card in {_direction}");
            return;
        }

        if (!_bulletImage)
        {
            print($"Bullet image is null in {_direction}");
            return;
        }

        _bulletImage.enabled = true;
        _bulletImage.sprite = cardToSet.cardIcon;
        _bulletImage.SetNativeSize();
        SetEnergyNodes(cardToSet.cardCost);

        print($"Bullet {_direction} activated with {cardToSet.cardName}");
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
        if (_energyNodesPool)
        {
            _energyNodesPool.SetAllObjectsInactive();
        }

        if (_bulletImage)
        {
            _bulletImage.enabled = false;
        }

        if (_numberImage)
        {
            _numberImage.enabled = false;
        }
    }

    private void SetEnergyNodes(int manaCost)
    {
        // Ensure the number image component exists
        if (!_numberImage)
        {
            Debug.LogError($"Number image is null in {_direction}");
            return;
        }

        // Validate the mana cost and sprite array
        if (_numberSprites == null || _numberSprites.Length < 10)
        {
            Debug.LogError($"Number sprites array is not properly set up in {_direction}. Requires 10 sprites (0-9).");
            return;
        }

        // Clear any existing energy nodes (legacy dot system)
        if (_energyNodesPool)
        {
            _energyNodesPool.SetAllObjectsInactive();
        }

        // Clamp mana cost to valid range (0-9)
        int displayCost = Mathf.Clamp(manaCost, 0, 9);

        // Enable and set the number sprite
        _numberImage.enabled = true;
        _numberImage.sprite = _numberSprites[displayCost];
        // Removed SetNativeSize() to prevent automatic size adjustment
        // Removed automatic positioning to let the number stay where it's placed in the scene
        
        // Ensure number image renders on top by moving it to the end of the hierarchy
        _numberImage.transform.SetAsLastSibling();
        
        // Additional fix: Move the entire BulletUI to the end of its parent's children
        // This ensures this BulletUI (and its number) renders after other BulletUIs
        transform.SetAsLastSibling();

        print($"Set number display for {_direction}: {displayCost}");
    }
}