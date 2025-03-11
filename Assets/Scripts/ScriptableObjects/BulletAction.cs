using UnityEngine;
/// DISCLAIMER: This is not a finalized class. It should match with the real bullet-action parameters.
/// This SO will only be used to testing the Bullet/Revolver UI
[CreateAssetMenu(fileName = "BulletAction", menuName = "Scriptable Objects/BulletAction")]
public class BulletAction : ScriptableObject
{
    [SerializeField]
    private string _actionName;
    [SerializeField]
    private int _manaCost;
    [SerializeField]
    private Sprite _bulletSprite;

    public string ActionName => _actionName;
    public int ManaCost => _manaCost;
    public Sprite BulletSprite => _bulletSprite;
}
