using System;
using UnityEditor.EditorTools;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The amount of time it will trigger a countdown in Seconds.")]
    private float _timeUnit = 1;
    private float _localTimer;
    private bool _isTicking;
    private event Action<float> OnDownTick;
    public bool IsTicking { get => _isTicking; set => _isTicking = value; }

    void OnEnable()
    {
        _localTimer = _timeUnit;
    }
    public void InitializeTimer(bool startTicking, Action<float> triggerEvent)
    {
        _isTicking = startTicking;
        OnDownTick = triggerEvent;
    }
    void Update()
    {
        if (_isTicking)
        {
            _localTimer -= Time.deltaTime;
            if (_localTimer <= 0)
            {
                OnDownTick?.Invoke(1);
                _localTimer = _timeUnit;
            }
        }
    }
}
