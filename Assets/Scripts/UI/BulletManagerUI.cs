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
    public BulletDirection Direction => _direction;


    void Awake()
    {
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
        if (manaCost > 9 || manaCost < 0)
        {
            Debug.LogError($"Number sprites array is not properly set up in {_direction}. Requires 10 sprites (0-9).");

        }



        // Enable and set the number sprite
        _numberImage.enabled = true;
        _numberImage.sprite = _numberSprites[manaCost];
        print($"Set number display for {_direction}: {manaCost}");
    }
}