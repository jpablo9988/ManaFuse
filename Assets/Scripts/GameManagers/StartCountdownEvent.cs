using System;
using System.Collections.Generic;
using AudioSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

///
/// Counts down from a timer, activating and deactivating certain objects in a pattern.
/// Player Controls will be deactivated while in this timer.
/// This is done for flair.¨¨
public class StartCountdownEvent : MonoBehaviour
{
    [Serializable]
    internal struct InteractiveObjects
    {
        public GameObject _go;
        public bool _startsActive;
        public bool _leaveActive;
        public bool _willSwitchBetween;
    }
    [Serializable]
    internal struct InteractiveUI
    {
        public Canvas _ca;
        public bool _startsActive;
        public bool _leaveActive;
        public bool _willSwitchBetween;
    }
    [Header("References")]
    [SerializeField]
    private List<InteractiveObjects> _objects;
    [SerializeField]
    private List<InteractiveUI> _uiObjects;
    [Header("Settings")]
    [SerializeField]
    int _noBlinks = 2;
    [SerializeField]
    float _timeActive;
    [SerializeField]
    float _timeInactive;
    [SerializeField]
    private MusicTrack _levelMusicTrack;
    private InputManager _inputManager;
    float _currTimer, _currCycles;
    bool _isOnActive;
    void Awake()
    {
        _inputManager = GameContext.Instance.InputManager;
    }
    void Start()
    {
        GameContext.Instance.AudioManager.PlayMusicTrack(_levelMusicTrack, false); //Should not fade in - starts in-sync with blinking event.
    }

    void OnEnable()
    {
        //Deactivate Player Controls.
        _currTimer = _timeActive + _timeInactive;
        _currCycles = _noBlinks;
        _inputManager.ActivateCardInputs = false;
        _inputManager.ActivatePlayerInputs = false;
        _inputManager.ActivateUIInputs = false; _isOnActive = true;
        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i]._go.SetActive(_objects[i]._startsActive);
        }
        for (int i = 0; i < _uiObjects.Count; i++)
        {
            _uiObjects[i]._ca.enabled = _uiObjects[i]._startsActive;
        }
    }
    void OnDisable()
    {
        //Activate Player Controls.
        _inputManager.ActivateCardInputs = true;
        _inputManager.ActivatePlayerInputs = true;
        _inputManager.ActivateUIInputs = true;
        for (int i = 0; i < _objects.Count; i++)
        {

            _objects[i]._go.SetActive(_objects[i]._leaveActive); //Throwing error here. gO is being destroyed when trying to active it.
        }
        for (int i = 0; i < _uiObjects.Count; i++)
        {
            _uiObjects[i]._ca.enabled = _uiObjects[i]._leaveActive;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Reaches the _timeInactive portion of the pattern. Reverse enabled self.
        if (_currTimer <= _timeInactive && _isOnActive)
        {
            _isOnActive = false;
            SwitchObjects(false);
        }
        else if (_currTimer <= 0)
        {
            _currTimer = _timeActive + _timeInactive;
            _isOnActive = true;
            _currCycles--;
            if (_currCycles <= 0)
            {
                enabled = false;
                return;
            }
            SwitchObjects(false);
        }
        _currTimer -= Time.deltaTime;
    }
    private void SwitchObjects(bool guaranteeSwitch = true)
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            if (_objects[i]._willSwitchBetween || guaranteeSwitch)
                _objects[i]._go.SetActive(!_objects[i]._go.activeSelf);
        }
        for (int i = 0; i < _uiObjects.Count; i++)
        {
            if (_uiObjects[i]._willSwitchBetween)
                _uiObjects[i]._ca.enabled = !_uiObjects[i]._ca.enabled;
        }
    }
}
