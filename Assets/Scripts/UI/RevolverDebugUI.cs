using UnityEngine;
using UnityEngine.UI;

public class RevolverDebugUI : MonoBehaviour
{
    [SerializeField]
    private Button addSouth, addNorth, removeSouth, removeNorth;
    [SerializeField]
    private RevolverManagerUI revUI;
    [SerializeField]
    private BulletAction bulletTest;
    void OnEnable()
    {
        addSouth.onClick.AddListener(delegate { revUI.ReloadBullet(BulletDirection.SOUTH, bulletTest); });
        addNorth.onClick.AddListener(delegate { revUI.ReloadBullet(BulletDirection.NORTH, bulletTest); });
        removeSouth.onClick.AddListener(delegate { revUI.RemoveBullet(BulletDirection.SOUTH); });
        removeNorth.onClick.AddListener(delegate { revUI.RemoveBullet(BulletDirection.NORTH); });
    }
    void OnDisable()
    {
        addSouth.onClick.RemoveAllListeners();
        addNorth.onClick.RemoveAllListeners();
        removeSouth.onClick.RemoveAllListeners();
        removeNorth.onClick.RemoveAllListeners();

    }
}
