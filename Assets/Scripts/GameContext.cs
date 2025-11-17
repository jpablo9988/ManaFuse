using System;
using System.Collections.Generic;
using UnityEngine;
using CardSystem;

[DefaultExecutionOrder(-100)] //Wake up as early as possible to avoid race conditions
public class GameContext : Singleton<GameContext>
{
    [Header("Dependencies")] [SerializeField]
    private InputManager _inputManager;

    [SerializeField] private PlayerManager _playerManager;


    [Header("Dependency Settings")] [SerializeField]
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

        //Cache commonly used services
        GetService<InputManager>();
        GetService<PlayerManager>();
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

    public readonly struct Unit
    {
        public static readonly Unit Value = default;
    }


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
        if (d is Func<T> f)
        {
            result = f();
            return true;
        }

        Console.Error.WriteLine($"Query token {token} registered with different delegate type.");
        return false;
    }

    public T Ask<T>(Ask<T> token)
    {
        if (TryAsk(token, out T r)) return r;
        throw new KeyNotFoundException($"Query handler not found for {token}");
    }

    // --- Typed handler accessors (for caching) ---
    public bool TryGetActHandler<TArg>(Act<TArg> token, out Action<TArg> handler)
    {
        handler = null;
        if (_handlers.TryGetValue(token, out var d) && d is Action<TArg> a)
        {
            handler = a;
            return true;
        }

        return false;
    }

    public bool TryGetAskHandler<T>(Ask<T> token, out Func<T> handler)
    {
        handler = null;
        if (_handlers.TryGetValue(token, out var d) && d is Func<T> f)
        {
            handler = f;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Resolve-and-invoke an Act handler with caching. If missing, logs and returns.
    /// Usage: _ctx.InvokeAct(PlayerTokens.ChangeMana, ref _changeManaHandler, args);
    /// </summary>
    public void InvokeAct<TArg>(Act<TArg> token, ref Action<TArg> cached, TArg arg)
    {
        if (cached == null)
        {
            if (!TryGetActHandler(token, out var h))
            {
                Console.Error.WriteLine($"{token} handler not registered.");
                return;
            }

            cached = h;
        }

        cached(arg);
    }


    /// Resolve-and-invoke an Ask handler with caching. Returns defaultValue if missing.
    /// Usage: return _ctx.InvokeAsk(PlayerTokens.SprintState, ref _isSprintingQuery);
    public T InvokeAsk<T>(Ask<T> token, ref Func<T> cached, T defaultValue = default)
    {
        if (cached == null)
        {
            if (!TryGetAskHandler(token, out var q))
            {
                Console.Error.WriteLine($"{token} query not registered.");
                return defaultValue;
            }

            cached = q;
        }

        return cached();
    }

    /// Convenience overload for Unit-based (no-arg) acts stored as Action<Unit>.
    public void InvokeAct(Act<Unit> token, ref Action<Unit> cached)
    {
        if (cached == null)
        {
            if (!TryGetActHandler(token, out var h))
            {
                Console.Error.WriteLine($"{token} handler not registered.");
                return;
            }

            cached = h;
        }

        cached(Unit.Value);
    }


    // --- Actions ---
    public void AddAct<TArg>(Act<TArg> token, Action<TArg> handler)
    {
        if (token == null) throw new ArgumentNullException(nameof(token));
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        _handlers[token] = handler;
    }

    public bool TryAct<TArg>(Act<TArg> token, TArg arg)
    {
        if (!_handlers.TryGetValue(token, out var d)) return false;
        if (d is Action<TArg> a)
        {
            a(arg);
            return true;
        }
        Console.Error.WriteLine($"Act token {token} registered with different delegate type.");
        return false;
    }

    public void Act<TArg>(Act<TArg> token, TArg arg)
    {
        if (!TryAct(token, arg))
            throw new KeyNotFoundException($"Act handler not found for {token}");
    }

    //No-parameter Action overloads
    public void AddAct(Act<Unit> token, Action handler)
    {
        if (token == null) throw new ArgumentNullException(nameof(token));
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        // Wrap the parameterless Action into Action<Unit> so it fits the existing storage pattern.
        _handlers[token] = new Action<Unit>(_ => handler());
    }


    public bool TryAct(Act<Unit> token)
    {
        if (!_handlers.TryGetValue(token, out var d)) return false;
        if (d is Action<Unit> a)
        {
            a(Unit.Value);
            return true;
        }
        Console.Error.WriteLine($"Act token {token} registered with different delegate type.");
        return false;
    }


    public void Act(Act<Unit> token)
    {
        if (!TryAct(token))
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

        public InputAPI(GameContext ctx)
        {
            _ctx = ctx;
        }

        // cached handlers
        private Action<bool> _setPlayerInputsHandler;
        private Action<bool> _setCardInputsHandler;
        private Action<bool> _setUIInputsHandler;

        private Func<bool> _playerInputsActiveQuery;
        private Func<bool> _cardInputsActiveQuery;
        private Func<bool> _uiInputsActiveQuery;

        // Acts (single-line now)
        public void SetPlayerInputs(bool active) =>
            _ctx.InvokeAct(InputManager.InputTokens.SetPlayerInputs, ref _setPlayerInputsHandler, active);

        public void SetCardInputs(bool active) =>
            _ctx.InvokeAct(InputManager.InputTokens.SetCardInputs, ref _setCardInputsHandler, active);

        public void SetUIInputs(bool active) =>
            _ctx.InvokeAct(InputManager.InputTokens.SetUIInputs, ref _setUIInputsHandler, active);

        // Asks (single-line)
        public bool IsPlayerInputsActive() =>
            _ctx.InvokeAsk(InputManager.InputTokens.PlayerInputsActive, ref _playerInputsActiveQuery, false);

        public bool IsCardInputsActive() =>
            _ctx.InvokeAsk(InputManager.InputTokens.CardInputsActive, ref _cardInputsActiveQuery, false);

        public bool IsUIInputsActive() =>
            _ctx.InvokeAsk(InputManager.InputTokens.UIInputsActive, ref _uiInputsActiveQuery, false);
    }


    public class PlayerAPI
    {
        readonly GameContext _ctx;

        public PlayerAPI(GameContext ctx)
        {
            _ctx = ctx;
        }

        // cached typed delegates
        private Action<PlayerManager.PlayerTokens.ManaArgs> _changeManaHandler;
        private Action<PlayerManager.PlayerTokens.ManaArgsFloat> _changeManaByTickHandler;
        private Action<PlayerManager.PlayerTokens.DashArgs> _dashHandler;
        private Action<Vector2> _defaultDashHandler;

        private Func<bool> _isSprintingQuery;
        private Func<bool> _isTimerTickingQuery;
        private Func<Transform> _playerTransformQuery;
        private Func<int> _currentManaUnitsQuery;

        // Acts
        public void ChangeMana(int amount, bool includeRedSlider = true) =>
            _ctx.InvokeAct(PlayerManager.PlayerTokens.ChangeMana, ref _changeManaHandler,
                new PlayerManager.PlayerTokens.ManaArgs { Amount = amount, IncludeRedSlider = includeRedSlider });

        public void ChangeManaByTickUnit(float amount, bool includeRedSlider = true) =>
            _ctx.InvokeAct(PlayerManager.PlayerTokens.ChangeManaByTickUnit, ref _changeManaByTickHandler,
                new PlayerManager.PlayerTokens.ManaArgsFloat { Amount = amount, IncludeRedSlider = includeRedSlider });

        public void Dash(Vector2 input, float distance, float duration) =>
            _ctx.InvokeAct(PlayerManager.PlayerTokens.Dash, ref _dashHandler,
                new PlayerManager.PlayerTokens.DashArgs { Input = input, Distance = distance, Duration = duration });

        public void DefaultDash(Vector2 input) =>
            _ctx.InvokeAct(PlayerManager.PlayerTokens.DefaultDash, ref _defaultDashHandler, input);

        // Asks
        public bool IsSprinting() =>
            _ctx.InvokeAsk(PlayerManager.PlayerTokens.SprintState, ref _isSprintingQuery, false);

        public bool IsTimerTicking() =>
            _ctx.InvokeAsk(PlayerManager.PlayerTokens.TimerTicking, ref _isTimerTickingQuery, false);

        public Transform pTransform() =>
            _ctx.InvokeAsk(PlayerManager.PlayerTokens.PlayerTransform, ref _playerTransformQuery, null);

        public int CurrentManaUnits() =>
            _ctx.InvokeAsk(PlayerManager.PlayerTokens.CurrentManaUnits, ref _currentManaUnitsQuery, 0);
    }
}