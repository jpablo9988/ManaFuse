using UnityEngine;
using CardSystem;

public class GameContext : Singleton<GameContext>
{
    [Header("Dependencies")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private RevolverManagerUI ui_RevolverManager;
    [SerializeField] private AttackManager _attackManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private DeckManager _deckManager;
    [Header("Dependency Settings")]
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
    public AttackManager ProjectileManager
    {
        get
        {
            if (_attackManager == null)
            {
                this.GetComponentInScene<InputManager>(_createManagersIfMissing, out _inputManager);
                _inputManager.enabled = true;
            }
            return _attackManager;
        }
    }
    public PlayerManager Player
    {
        get
        {
            if (_playerManager == null)
            {
                this.GetComponentInScene(_createManagersIfMissing, out _playerManager);
                _playerManager.enabled = true;
            }
            return _playerManager;
        }
    }
    public DeckManager DeckManager
    {
        get
        {
            if (_deckManager == null)
            {
                this.GetComponentInScene<DeckManager>(_createManagersIfMissing, out _deckManager);
                _deckManager.enabled = true;
            }
            return _deckManager;
        }
    }
    override protected void Awake()
    {
        base.Awake();
        if (_inputManager == null) _inputManager = this.GetComponentInScene<InputManager>(_createManagersIfMissing, out _inputManager);
        ui_RevolverManager = this.GetComponentInScene<RevolverManagerUI>(false, out ui_RevolverManager);
        _playerManager = this.GetComponentInScene(false, out _playerManager);
        if (_attackManager == null) _attackManager = this.GetComponentInScene<AttackManager>(_createManagersIfMissing, out _attackManager);
        if (_deckManager == null) _deckManager = this.GetComponentInScene<DeckManager>(_createManagersIfMissing, out _deckManager);
    }

}
