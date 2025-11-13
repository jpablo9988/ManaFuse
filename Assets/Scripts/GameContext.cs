using System;
using System.Collections.Generic;
using UnityEngine;
using CardSystem;

[DefaultExecutionOrder(-100)] //Wake up as early as possible to avoid race conditions
public class GameContext : Singleton<GameContext>
{
    [Header("Dependencies")]
    [SerializeField]
    private InputManager _inputManager;
    [SerializeField] private PlayerManager _playerManager;


    [Header("Dependency Settings")]
    [SerializeField]
    private bool _createManagersIfMissing = true;
    
    // Dictionary to store all of our managers
    readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
    
    protected override void Awake()
    {
        base.Awake();
        
        // !!Register your managers here!!
        if (_inputManager) RegisterService(_inputManager);
        if (_playerManager) RegisterService(_playerManager);


        // initialize API hubs
        Player = new PlayerAPI(this);
        Input = new InputAPI(this);
    }
    
    // -- Registration Service --
    
    public void RegisterService<T>(T service) where T : Component
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        _services[typeof(T)] = service;
    }

    //Try to get services from our dictionary
    public bool TryGetService<T>(out T service) where T : Component
    {
        if (_services.TryGetValue(typeof(T), out var obj))
        {
            service = obj as T;
            return service != null;
        }

        service = null;
        return false;
    }

    //Get services within our scene
    public T GetService<T>() where T : Component
    {
        //Try to grab from our registered services first (Dictionary)
        if (TryGetService<T>(out var service)) return service;

        //lazy fallback if some services still aren't found. This only happens once.
        var found = UnityEngine.Object.FindFirstObjectByType<T>();
        if (found != null)
        {
            RegisterService(found);
            return found;
        }
        
        //Give up and create those missing managers
        if (_createManagersIfMissing)
        {
            var go = new GameObject(typeof(T).Name + ".Auto");
            var comp = go.AddComponent<T>();
            RegisterService(comp);
            return comp;
        }

        // not found
        return null;
    }


    // -- End Of Service Registration --
    
    /////////////////////////////////////////////////////////////////////////////////
                            // Start of Ask/Act Registration //
    /////////////////////////////////////////////////////////////////////////////////

    //Initialize Our Dictionary
    private readonly Dictionary<object, Delegate> _handlers = new Dictionary<object, Delegate>();
    
    
    // --- Asks ---
    public void AddAsk<T>(Ask<T> token, Func<T> handler)
    {
        if (token == null) throw new ArgumentNullException(nameof(token));
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        _handlers[token] = handler;
    }

    public bool TryAsk<T>(Ask<T> token, out T result)
    {
        result = default;
        if (!_handlers.TryGetValue(token, out var d)) return false;
        if (d is Func<T> f) { result = f(); return true; }
        Debug.LogError($"Query token {token} registered with different delegate type.");
        return false;
    }

    public T Ask<T>(Ask<T> token)
    {
        if (TryAsk(token, out T r)) return r;
        throw new KeyNotFoundException($"Query handler not found for {token}");
    }
    

    // --- Actions ---
    public void AddAct<TArg>(Act<TArg> token, Action<TArg> handler)
    {
        if (token == null) throw new ArgumentNullException(nameof(token));
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        _handlers[token] = handler;

        _handlers[token] = handler;
    }

    public bool TryAct<TArg>(Act<TArg> token, TArg arg)
    {
        if (!_handlers.TryGetValue(token, out var d)) return false;
        if (d is Action<TArg> a) { a(arg); return true; }
        Debug.LogError($"Act token {token} registered with different delegate type.");
        return false;
    }

    public void Act<TArg>(Act<TArg> token, TArg arg)
    {
        if (!TryAct(token, arg))
            throw new KeyNotFoundException($"Act handler not found for {token}");
    }
    /////////////////////////////////////////////////////////////////////////////////
                            // End of Ask/Act Registration //
    /////////////////////////////////////////////////////////////////////////////////
    
    
    //Without API implementation, you can use TokenExtensions to call Asks and Acts more efficiently.
    //Example: ChangeManaByTickUnit.Invoke(new() { Amount = manaReward, IncludeRedSlider = true });
    
    
    
    //  --  Api Implementations  --
    //Ensure that all APIs are initialized in Awake above!
    
    public PlayerAPI Player { get; private set; }
    public InputAPI Input { get; private set; }
    
    // Backwards-compatible accessor for the revolver UI manager used by card systems
    public RevolverManagerUI UIRevolverManager => GetService<RevolverManagerUI>();

    public class InputAPI
    {
        readonly GameContext _ctx;
        public InputAPI(GameContext ctx) { _ctx = ctx; }

        //Acts
        public void SetPlayerInputs(bool active)
            => _ctx.Act(InputManager.InputTokens.SetPlayerInputs, active);

        public void SetCardInputs(bool active)
            => _ctx.Act(InputManager.InputTokens.SetCardInputs, active);

        public void SetUIInputs(bool active)
            => _ctx.Act(InputManager.InputTokens.SetUIInputs, active);

        //Asks
        public bool IsPlayerInputsActive()
            => _ctx.Ask(InputManager.InputTokens.PlayerInputsActive);

        public bool IsCardInputsActive()
            => _ctx.Ask(InputManager.InputTokens.CardInputsActive);

        public bool IsUIInputsActive()
            => _ctx.Ask(InputManager.InputTokens.UIInputsActive);
    }

    public class PlayerAPI
    {
        readonly GameContext _ctx;
        public PlayerAPI(GameContext ctx) { _ctx = ctx; }

        //Acts
        public void ChangeMana(int amount, bool includeRedSlider = true)
            => _ctx.Act(PlayerManager.PlayerTokens.ChangeMana, new PlayerManager.PlayerTokens.ManaArgs {
                Amount = amount,
                IncludeRedSlider = includeRedSlider
            });

        public void ChangeManaByTickUnit(float amount, bool includeRedSlider = true)
            => _ctx.Act(PlayerManager.PlayerTokens.ChangeManaByTickUnit, new PlayerManager.PlayerTokens.ManaArgsFloat {
                Amount = amount,
                IncludeRedSlider = includeRedSlider
            });

        public void Dash(Vector2 input, float distance, float duration)
            => _ctx.Act(PlayerManager.PlayerTokens.Dash, new PlayerManager.PlayerTokens.DashArgs {
                Input = input,
                Distance = distance,
                Duration = duration
            });

        public void DefaultDash(Vector2 input)
            => _ctx.Act(PlayerManager.PlayerTokens.DefaultDash, input);

        //Asks
        public bool IsSprinting()
            => _ctx.Ask(PlayerManager.PlayerTokens.SprintState);

        public bool IsTimerTicking()
            => _ctx.Ask(PlayerManager.PlayerTokens.TimerTicking);
    }
}
