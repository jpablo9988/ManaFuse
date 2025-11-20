using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveSystem;

[RequireComponent(typeof(PersistentId))]
public class GameProgressTracker : MonoBehaviour, ISaveable
{
    [Serializable]
    internal struct ObjectReference
    {
        public GameObject go;
        public bool disableOnStart;
        public bool enableOnEnd;
    }
    [SerializeField]
    private string _enemyTagName = "Enemy";
    [SerializeField]
    private ObjectReference[] _winPlayerObjects;
    [SerializeField]
    private ObjectReference[] _losePlayerObjects;
    private int noEnemiesInScene = 0;
    public bool winByEnemiesKilled = true;
    private bool isVictoryAchieved = false;
    private PersistentId _persistentId;

    public bool IsVictoryAchieved
    {
        get { return isVictoryAchieved; }
        set
        {
            isVictoryAchieved = true;
            if (isVictoryAchieved) OnVictory();
        }
    }

    void Awake()
    {
        _persistentId = GetComponent<PersistentId>();
    }

    void Start()
    {
        noEnemiesInScene = GameObject.FindGameObjectsWithTag(_enemyTagName).Count();
        foreach (ObjectReference obj in _winPlayerObjects)
        {
            if (obj.disableOnStart)
            {
                obj.go.SetActive(false);
            }
        }
        foreach (ObjectReference obj in _losePlayerObjects)
        {
            if (obj.disableOnStart)
            {
                obj.go.SetActive(false);
            }
        }

    }
    void OnEnable()
    {
        EnemyAI.OnDeathEnemy += OnDeathEnemy;
        PlayerManager.OnDeathPlayer += OnDeathPlayer;

        // Register with save system
        SaveGameManager.Instance.Register(this);
    }
    void OnDisable()
    {
        EnemyAI.OnDeathEnemy -= OnDeathEnemy;
        PlayerManager.OnDeathPlayer -= OnDeathPlayer;

        // Unregister from save system
        SaveGameManager.Instance.Unregister(this);
    }
    private void OnDeathEnemy()
    {
        noEnemiesInScene--;
        if (noEnemiesInScene <= 0 && winByEnemiesKilled)
        {
            OnVictory();
        }
    }
    private void OnVictory()
    {
        //Disable all Player Inputs.
        GameContext.Instance.InputManager.ActivateCardInputs = false;
        GameContext.Instance.InputManager.ActivatePlayerInputs = false;
        foreach (ObjectReference obj in _winPlayerObjects)
        {
            obj.go.SetActive(obj.enableOnEnd);
        }
    }
    private void OnDeathPlayer()
    {
        //Disable all Player Inputs.
        GameContext.Instance.InputManager.ActivateCardInputs = false;
        GameContext.Instance.InputManager.ActivatePlayerInputs = false;
        foreach (ObjectReference obj in _losePlayerObjects)
        {
            obj.go.SetActive(obj.enableOnEnd);
        }

    }

    // ISaveable Implementation
    public string SaveId => _persistentId ? _persistentId.Id : string.Empty;

    public object CaptureState()
    {
        return new ProgressSaveData
        {
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex,
            currentSceneName = SceneManager.GetActiveScene().name,
            isVictoryAchieved = this.isVictoryAchieved,
            enemiesRemaining = this.noEnemiesInScene
        };
    }

    public void RestoreState(object state)
    {
        if (state is ProgressSaveData data)
        {
            this.isVictoryAchieved = data.isVictoryAchieved;
            this.noEnemiesInScene = data.enemiesRemaining;

            Debug.Log($"Progress restored: Scene={data.currentSceneName}, Enemies={data.enemiesRemaining}, Victory={data.isVictoryAchieved}");
        }
    }

    [Serializable]
    private class ProgressSaveData
    {
        public int currentSceneIndex;
        public string currentSceneName;
        public bool isVictoryAchieved;
        public int enemiesRemaining;
    }
}
