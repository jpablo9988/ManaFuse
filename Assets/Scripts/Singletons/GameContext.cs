using UnityEngine;
using CardSystem;
using AudioSystem;

public class GameContext : Singleton<GameContext>
{
    [Header("Dependencies")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private RevolverManagerUI ui_RevolverManager;
    [SerializeField] private AttackManager _attackManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private PauseHandler _pauseManager;
    [SerializeField] private AudioManager _audioManager;

    [Header("Dependency Settings")]
    [SerializeField] private bool _createManagersIfMissing = true;
    public InputManager InputManager
    {
        get
        {
            if (_inputManager) return _inputManager;
            this.GetComponentInScene<InputManager>(_createManagersIfMissing, out _inputManager);
            _inputManager.enabled = true;
            return _inputManager;
        }
    }
    public RevolverManagerUI UIRevolverManager
    {
        get
        {
            if (ui_RevolverManager) return ui_RevolverManager;
            this.GetComponentInScene<RevolverManagerUI>(_createManagersIfMissing, out ui_RevolverManager);
            ui_RevolverManager.enabled = true;
            return ui_RevolverManager;
        }
    }
    public AttackManager ProjectileManager
    {
        get
        {
            if (_attackManager) return _attackManager;
            this.GetComponentInScene<InputManager>(_createManagersIfMissing, out _inputManager);
            _inputManager.enabled = true;
            return _attackManager;
        }
    }
    public PlayerManager Player
    {
        get
        {
            if (_playerManager) return _playerManager;
            this.GetComponentInScene(_createManagersIfMissing, out _playerManager);
            _playerManager.enabled = true;
            return _playerManager;
        }
    }
    public DeckManager DeckManager
    {
        get
        {
            if (_deckManager) return _deckManager;
            this.GetComponentInScene<DeckManager>(_createManagersIfMissing, out _deckManager);
            _deckManager.enabled = true;
            return _deckManager;
        }
    }
    public PauseHandler PauseHandler
    {
        get
        {
            if (_pauseManager) return _pauseManager;
            this.GetComponentInScene<PauseHandler>(_createManagersIfMissing, out _pauseManager);
            _pauseManager.enabled = true;
            return _pauseManager;
        }
    }
    public AudioManager AudioManager
    {
        get
        {
            if (_audioManager) return _audioManager;
            this.GetComponentInScene<AudioManager>(_createManagersIfMissing, out _audioManager);
            _audioManager.enabled = true;
            return _audioManager;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        if (!_inputManager) _inputManager = this.GetComponentInScene<InputManager>(_createManagersIfMissing, out _inputManager);
        ui_RevolverManager = this.GetComponentInScene<RevolverManagerUI>(false, out ui_RevolverManager);
        _playerManager = this.GetComponentInScene(false, out _playerManager);
        if (!_attackManager) _attackManager = this.GetComponentInScene<AttackManager>(_createManagersIfMissing, out _attackManager);
        if (!_deckManager) _deckManager = this.GetComponentInScene<DeckManager>(_createManagersIfMissing, out _deckManager);
        if (!_audioManager) _audioManager = this.GetComponentInScene<AudioManager>(_createManagersIfMissing, out _audioManager);



        //Check for missing dependencies if they have failed to have been created! 
#if UNITY_EDITOR
        var missingDependencies = "";

        if (!_inputManager) { missingDependencies += "InputManager "; }
        if (!ui_RevolverManager) { missingDependencies += "RevolverManagerUI "; }
        if (!_playerManager) { missingDependencies += "PlayerManager "; }
        if (!_attackManager) { missingDependencies += "AttackManager "; }
        if (!_deckManager) { missingDependencies += "DeckManager "; }
        if (!_audioManager) { missingDependencies += "AudioManager "; }

        //Report missing dependencies and stop play mode if in Editor
        if (string.IsNullOrEmpty(missingDependencies)) return;
        Debug.LogError("UNABLE TO FIND / CREATE DEPENDENCIES: " + missingDependencies);
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
