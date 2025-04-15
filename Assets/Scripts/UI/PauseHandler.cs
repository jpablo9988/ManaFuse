using System;
using UnityEditor;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    [Serializable]
    internal struct ObjectReference
    {
        public GameObject go;
        public bool enableOnPause;
        public bool disableOnStart;
    }
    [SerializeField]
    private ObjectReference[] _objs;
    private bool _isPaused;
    public bool IsPaused => _isPaused;
    void Start()
    {
        foreach (ObjectReference obj in _objs)
        {
            if (obj.disableOnStart) obj.go.SetActive(false);
        }
    }
    public void ChangePauseState(bool pause)
    {
        _isPaused = pause;
        SwitchObjectStates(pause);
    }
    public void ChangePauseState()
    {
        _isPaused = !_isPaused;
        SwitchObjectStates(_isPaused);
    }

    private void SwitchObjectStates(bool pause)
    {
        foreach (ObjectReference obj in _objs)
        {
            if (pause)
            {
                obj.go.SetActive(obj.enableOnPause);
            }
            else
            {
                obj.go.SetActive(!obj.enableOnPause);
            }
        }
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}

