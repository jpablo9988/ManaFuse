using UnityEngine;

public class GameContext : Singleton<GameContext>
{
    [Header("Dependencies")]
    [SerializeField] private InputManager _inputManager;
    [Header("Dependecy Settings")]
    [SerializeField] private bool _createManagersIfMissing = true;
    public InputManager InputManager
    {
        get
        {
            if (_inputManager == null)
            {
                return this.GetComponentInScene<InputManager>(_createManagersIfMissing);
            }
            return _inputManager;
        }
    }
    override protected void Awake()
    {
        base.Awake();
        if (_inputManager == null) _inputManager = this.GetComponentInScene<InputManager>(_createManagersIfMissing);
    }

}
