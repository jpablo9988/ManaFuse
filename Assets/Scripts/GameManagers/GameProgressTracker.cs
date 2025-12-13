using System;
using System.Linq;
using UnityEngine;

public class GameProgressTracker : MonoBehaviour
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
    }
    void OnDisable()
    {
        EnemyAI.OnDeathEnemy -= OnDeathEnemy;
        PlayerManager.OnDeathPlayer -= OnDeathPlayer;
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
}
