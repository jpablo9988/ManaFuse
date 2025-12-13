using UnityEngine;
using UnityEditor;
using SaveSystem;

/// <summary>
/// Editor utility to automatically add PersistentId components to prefabs that need them.
/// </summary>
public class AddPersistentIds : EditorWindow
{
    [MenuItem("Tools/Save System/Add PersistentId to Prefabs")]
    public static void AddPersistentIdsToPrefabs()
    {
        // Find all prefabs in the project
        string[] prefabPaths = new string[]
        {
            "Assets/Prefabs/Player.prefab",
            "Assets/Prefabs/Deck.prefab"
        };

        int addedCount = 0;
        int skippedCount = 0;

        foreach (string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            if (prefab == null)
            {
                Debug.LogWarning($"Could not find prefab at {path}");
                continue;
            }

            // Check if it already has PersistentId
            if (prefab.GetComponent<PersistentId>() != null)
            {
                Debug.Log($"{prefab.name} already has PersistentId component");
                skippedCount++;
                continue;
            }

            // Add PersistentId component
            GameObject prefabInstance = PrefabUtility.LoadPrefabContents(path);
            
            if (prefabInstance.GetComponent<PersistentId>() == null)
            {
                prefabInstance.AddComponent<PersistentId>();
                PrefabUtility.SaveAsPrefabAsset(prefabInstance, path);
                Debug.Log($"Added PersistentId to {prefab.name}");
                addedCount++;
            }
            else
            {
                Debug.Log($"{prefab.name} already has PersistentId component");
                skippedCount++;
            }

            PrefabUtility.UnloadPrefabContents(prefabInstance);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        EditorUtility.DisplayDialog(
            "Add PersistentIds Complete",
            $"Added PersistentId to {addedCount} prefab(s).\nSkipped {skippedCount} prefab(s) that already had it.",
            "OK"
        );
    }
}

