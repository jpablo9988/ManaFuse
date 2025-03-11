using Unity.VisualScripting;
using UnityEngine;

public static class MonoBehaviourExtension
{
    public static T GetComponentInScene<T>(this MonoBehaviour self) where T : MonoBehaviour
    {
        T _searchedScript = Object.FindFirstObjectByType<T>();
        if (_searchedScript == null)
        {

            GameObject GO = new();
            _searchedScript = GO.AddComponent<T>();
            GO.name = "(Component) " + typeof(T).ToString();
            Debug.LogWarning($"An instance of {_searchedScript.GetType()} is missing in scene, creating a new one.");


        }
        return _searchedScript;
    }
    public static T GetComponentInScene<T>(this MonoBehaviour self, bool createObjectIfMissing, out T Result) where T : MonoBehaviour
    {
        Result = Object.FindFirstObjectByType<T>();
        if (Result == null)
        {
            if (createObjectIfMissing)
            {
                GameObject GO = new();
                Result = GO.AddComponent<T>();
                GO.name = "(Component) " + typeof(T).ToString();
                Debug.LogWarning($"An instance of {Result.GetType()} is missing in scene, creating a new one.");
            }
            else
            {
                Debug.LogWarning($"An instance of an object is missing in scene and will not be created. Returning null");
                return null;
            }

        }
        return Result;
    }
}
