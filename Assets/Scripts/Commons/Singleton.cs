using UnityEngine;
/// Aggregates  all the basic Singleton Pattern needs into a single inheritable abstract class.
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    GameObject singleton = new();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = "(singleton) " + typeof(T).ToString();
                    Debug.LogWarning("An instance of " + typeof(T) + " is missing in scene, creating a new one.");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(this);
            Debug.LogWarning($"An instance of {GetType().Name} already exists. Destroying this instance.");
        }
    }
}