using CardSystem;
using UnityEngine;
using UnityEngine.UI;

public class RevolverDebugUI : MonoBehaviour
{
    [SerializeField]
    private Button addSouth, addNorth, removeSouth, removeNorth;
    [SerializeField]
    private RevolverManagerUI revUI;
    [SerializeField]
    private Card cardTest;
    void OnEnable()
    {
        addSouth.onClick.AddListener(delegate { revUI.LoadCard(BulletDirection.SOUTH, cardTest); });
        addNorth.onClick.AddListener(delegate { revUI.LoadCard(BulletDirection.NORTH, cardTest); });
        removeSouth.onClick.AddListener(delegate { revUI.DiscardCard(BulletDirection.SOUTH); });
        removeNorth.onClick.AddListener(delegate { revUI.DiscardCard(BulletDirection.NORTH); });
    }
    void OnDisable()
    {
        addSouth.onClick.RemoveAllListeners();
        addNorth.onClick.RemoveAllListeners();
        removeSouth.onClick.RemoveAllListeners();
        removeNorth.onClick.RemoveAllListeners();

    }
}
