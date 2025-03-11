using UnityEngine;

public class GameContext : Singleton<GameContext>
{
    [Header("Dependencies")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private AttackManager _attackManager;
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
    override protected void Awake()
    {
        base.Awake();
        if (_inputManager == null) _inputManager = this.GetComponentInScene<InputManager>(_createManagersIfMissing, out _inputManager);
        if (_attackManager == null) _attackManager = this.GetComponentInScene<AttackManager>(_createManagersIfMissing, out _attackManager);
    }

}
