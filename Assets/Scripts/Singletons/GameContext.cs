using UnityEngine;

public class GameContext : Singleton<GameContext>
{
    [Header("Dependencies")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private RevolverManagerUI ui_RevolverManager;
    [SerializeField] private ManafuseBar ui_ManaManager;
    [Header("Dependecy Settings")]
    [SerializeField] private bool _createManagersIfMissing = true;
    public InputManager InputManager
    {
        get
        {
            if (_inputManager == null)
            {
                this.GetComponentInScene<InputManager>(_createManagersIfMissing, out _inputManager);
                _inputManager.enabled = true;
            }
            return _inputManager;
        }
    }
    public RevolverManagerUI UIRevolverManager
    {
        get
        {
            if (ui_RevolverManager == null)
            {
                this.GetComponentInScene<RevolverManagerUI>(_createManagersIfMissing, out ui_RevolverManager);
                ui_RevolverManager.enabled = true;
            }
            return ui_RevolverManager;
        }
    }
    public ManafuseBar UIManafuseBar
    {
        get
        {
            if (_inputManager == null)
            {
                this.GetComponentInScene<ManafuseBar>(_createManagersIfMissing, out ui_ManaManager);
                ui_ManaManager.enabled = true;
            }
            return ui_ManaManager;
        }
    }
    override protected void Awake()
    {
        base.Awake();
        if (_inputManager == null) _inputManager = this.GetComponentInScene<InputManager>(_createManagersIfMissing, out _inputManager);
        ui_ManaManager = this.GetComponentInScene<ManafuseBar>(false, out ui_ManaManager);
        ui_RevolverManager = this.GetComponentInScene<RevolverManagerUI>(false, out ui_RevolverManager);
    }

}
