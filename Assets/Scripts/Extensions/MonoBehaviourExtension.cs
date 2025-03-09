using Unity.VisualScripting;
using UnityEngine;

public static class MonoBehaviourExtension
{
    public static T GetComponentInScene<T>(this MonoBehaviour self, bool createObjectIfMissing) where T : MonoBehaviour
    {
        T _searchedScript = Object.FindFirstObjectByType<T>();
        if (_searchedScript == null)
        {
            if (createObjectIfMissing)
            {
                GameObject GO = new();
                _searchedScript = GO.AddComponent<T>();
                GO.name = "(Component) " + typeof(T).ToString();
                Debug.LogWarning($"An instance of {_searchedScript.GetType()} is missing in scene, creating a new one.");
            }
            else
            {
                Debug.LogError($"An instance of {_searchedScript.GetType()} is missing in scene. Returning Null.");
                return null;
            }

        }
        return _searchedScript;
    }
}
