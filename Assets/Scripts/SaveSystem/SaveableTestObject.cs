
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using SaveSystem;
using UnityEngine;

/// <summary>
/// Testing class and Example for the Save System.
/// TODO: Proper Testing Infrastructure needed for later system tests.
/// - Juan Pablo Amorocho
/// </summary>
public class SaveableTestObject : MonoBehaviour, ISaveable
{
    [SerializeField]
    private string saveId;
    public string SaveId => saveId;
    [Serializable]
    public struct TestData
    {
        public List<string> objectIds;
    }

    [SerializeField]
    private TestData testData;

    public object CaptureState()
    {
        return testData;
    }

    public void RestoreState(object state)
    {
        testData = (TestData)state;
    }

    public void RegisterToSaveManager()
    {
        SaveGameManager.Instance.Register(this);
    }
    public void UnregisterFromSaveManager()
    {
        SaveGameManager.Instance.Unregister(this);
    }
}
#endif
