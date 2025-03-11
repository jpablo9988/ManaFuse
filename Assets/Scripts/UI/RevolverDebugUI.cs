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
    [SerializeField]
    private GameObject projectilePrefab1;
    [SerializeField]
    private GameObject projectilePrefab2;
    [SerializeField]
    private Transform spawnPoint;
    void OnEnable()
    {
        addSouth.onClick.AddListener(delegate { revUI.ReloadBullet(BulletDirection.SOUTH, bulletTest); });
        addNorth.onClick.AddListener(delegate { revUI.ReloadBullet(BulletDirection.NORTH, bulletTest); });
        //removeSouth.onClick.AddListener(delegate { revUI.RemoveBullet(BulletDirection.SOUTH); });
        removeSouth.onClick.AddListener(delegate {
            revUI.RemoveBullet(BulletDirection.SOUTH);
            Instantiate(projectilePrefab2, spawnPoint.position, spawnPoint.rotation);
        });
        //removeNorth.onClick.AddListener(delegate { revUI.RemoveBullet(BulletDirection.NORTH); });
        removeNorth.onClick.AddListener(delegate { revUI.RemoveBullet(BulletDirection.NORTH); 
            Instantiate(projectilePrefab1, spawnPoint.position, spawnPoint.rotation);
        });
    }
    void OnDisable()
    {
        addSouth.onClick.RemoveAllListeners();
        addNorth.onClick.RemoveAllListeners();
        removeSouth.onClick.RemoveAllListeners();
        removeNorth.onClick.RemoveAllListeners();

    }
}
