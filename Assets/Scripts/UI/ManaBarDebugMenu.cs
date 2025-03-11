using UnityEngine;
using UnityEngine.UI;

public class ManaBarDebugMenu : MonoBehaviour
{
    [SerializeField]
    private Button removeUnit, removeUnitNoRed, removeTick, addUnit;
    [SerializeField]
    private ManafuseBar testBar;
    void OnEnable()
    {
        removeUnit.onClick.AddListener(delegate { testBar.ChangeByUnit(-1, true); });
        removeUnitNoRed.onClick.AddListener(delegate { testBar.ChangeByUnit(-1, false); });
        removeTick.onClick.AddListener(delegate { testBar.ChangeByTick(-1, true); });
        addUnit.onClick.AddListener(delegate { testBar.ChangeByUnit(2, false); });
    }
    void OnDisable()
    {
        removeUnit.onClick.RemoveAllListeners();
        removeUnitNoRed.onClick.RemoveAllListeners();
        removeTick.onClick.RemoveAllListeners();
        addUnit.onClick.RemoveAllListeners();

    }
}
