using System;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerManager.PlayerTokens;
using Object = System.Object;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Timer _timer;
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _playerTransform;
    

    private void Awake()
    {
        if (GameContext.Instance == null)
        {
            GameContext.Instance.RegisterService(this);
        }
        
        _movement = GetComponent<PlayerMovement>();
        _timer = GetComponent<Timer>();
        _stats = GetComponent<PlayerStats>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.transform;
        
        
        var ctx = GameContext.Instance;
        
        // Initialize Asks
        ctx.AddAsk(SprintState, () => _movement.IsSprinting);
        ctx.AddAsk(TimerTicking, () => _timer.IsTicking);
        ctx.AddAsk(PlayerTransform, () => _playerTransform);
        ctx.AddAsk(CurrentManaUnits, () => _stats.CurrentManaUnits);
        //GameContext.Instance.AddAsk(PlayerTokens.TimerTime, () => _timer.Time);

        // Initialize Acts
        ctx.AddAct(DefaultDash, (Vector2 input) =>
            _movement.InitiateSprint(input));

        ctx.AddAct(Dash, (PlayerTokens.DashArgs args) => 
            _movement.InitiateSprint(args.Input, args.Distance, args.Duration));
        
        ctx.AddAct(ChangeMana, (PlayerTokens.ManaArgs args) =>
            _stats.ChangeMana(args.Amount, args.IncludeRedSlider));

        ctx.AddAct(ChangeManaByTickUnit, (PlayerTokens.ManaArgsFloat args) =>
            _stats.ChangeManaByTickUnit(args.Amount, args.IncludeRedSlider));
        
    }
    
    
    public static class PlayerTokens
    {
        // Define Asks
        public static readonly Ask<bool> SprintState  = new Ask<bool>("Player.SprintState");
        public static readonly Ask<bool> TimerTicking = new Ask<bool>("Player.TimerTicking");
        public static readonly Ask<Transform> PlayerTransform = new Ask<Transform>("Player.Transform");
        public static readonly Ask<int> CurrentManaUnits = new Ask<int>("Player.CurrentManaUnits");
        //public static readonly Ask<float> TimerTime   = new Ask<float>("Player.TimerTime");
        
        //Define Args (Required for multi-arg Acts)
        public struct DashArgs { public Vector2 Input; public float Distance; public float Duration; }
        public struct ManaArgs {public int Amount; public bool IncludeRedSlider; }
        public struct ManaArgsFloat {public float Amount; public bool IncludeRedSlider; }

        //Define Acts
        public static readonly Act<DashArgs> Dash = new Act<DashArgs>("Player.Dash");
        public static readonly Act<Vector2> DefaultDash = new Act<Vector2>("Player.DefaultDash");
        
        public static readonly Act<ManaArgs> ChangeMana = new Act<ManaArgs>("Player.ChangeMana");
        public static readonly Act<ManaArgsFloat> ChangeManaByTickUnit = new Act<ManaArgsFloat>("Player.ChangeManaByTickUnit");
        
    }
}
