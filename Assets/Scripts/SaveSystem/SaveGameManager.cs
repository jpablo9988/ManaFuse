using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Central coordinator for capturing and restoring ISaveable state.
    /// Handles registration, file I/O, and basic slot metadata.
    /// </summary>
    public sealed class SaveGameManager : MonoBehaviour
    {
        //TODO: This class shouldn't be a Singleton. It should be accessed through GameContext.
        private static SaveGameManager _instance;
        public static SaveGameManager Instance
        {
            get
            {
                if (_instance) return _instance;
                _instance = FindFirstObjectByType<SaveGameManager>();
                if (_instance) return _instance;

                var go = new GameObject("SaveGameManager");
                _instance = go.AddComponent<SaveGameManager>();
                return _instance;
            }
        }

        [SerializeField] private string saveFolderName = "Saves";
        [SerializeField] private bool prettyPrintJson = true;

        private readonly Dictionary<string, ISaveable> _saveables = new();

        private string SaveDirectory =>
            Path.Combine(Application.persistentDataPath, saveFolderName);

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureSaveDirectory();
        }

        private void OnApplicationQuit()
        {
            // Clean up on quit to prevent editor warnings
            _saveables.Clear();
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            // Clean up when exiting play mode in editor
            if (_instance == this)
            {
                _instance = null;
            }
        }
#endif

        /// <summary>
        /// Registers an ISaveable so it participates in save/load.
        /// </summary>
        public void Register(ISaveable saveable)
        {
            if (saveable == null)
            {
                Debug.LogWarning("Attempted to register null ISaveable.");
                return;
            }

            if (string.IsNullOrEmpty(saveable.SaveId))
            {
                var behaviour = saveable as MonoBehaviour;
                var sourceName = behaviour ? behaviour.name : saveable.GetType().Name;
                Debug.LogWarning($"ISaveable on {sourceName} has no SaveId.");
                return;
            }

            _saveables[saveable.SaveId] = saveable;
        }

        /// <summary>
        /// Removes a previously registered ISaveable.
        /// </summary>
        public void Unregister(ISaveable saveable)
        {
            if (saveable == null) return;
            if (string.IsNullOrEmpty(saveable.SaveId)) return;

            if (_saveables.TryGetValue(saveable.SaveId, out var existing) && existing == saveable)
            {
                _saveables.Remove(saveable.SaveId);
            }
        }

        /// <summary>
        /// Captures all registered state and writes it to disk.
        /// </summary>
        public bool Save(string slotName)
        {
            if (string.IsNullOrWhiteSpace(slotName))
            {
                Debug.LogError("Save slot name is invalid.");
                return false;
            }

            EnsureSaveDirectory();

            var slotData = new SaveSlotData
            {
                slotName = slotName,
                savedAtUtc = DateTime.UtcNow.ToString("o"),
                states = new List<StateRecord>()
            };

            foreach (var pair in _saveables)
            {
                try
                {
                    var state = pair.Value.CaptureState();
                    if (state == null) continue;

                    var record = new StateRecord
                    {
                        id = pair.Key,
                        type = state.GetType().AssemblyQualifiedName,
                        json = JsonUtility.ToJson(state)
                    };
                    slotData.states.Add(record);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to capture state for {pair.Key}: {ex}");
                }
            }

            var path = GetSlotPath(slotName);
            try
            {
                var json = JsonUtility.ToJson(slotData, prettyPrintJson);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save slot {slotName}: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Loads the specified slot and pushes captured state into any registered saveables.
        /// </summary>
        public bool Load(string slotName)
        {
            var path = GetSlotPath(slotName);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"No save file found for slot {slotName}.");
                return false;
            }

            try
            {
                var json = File.ReadAllText(path);
                var slotData = JsonUtility.FromJson<SaveSlotData>(json);
                if (slotData?.states == null)
                {
                    Debug.LogWarning($"Save slot {slotName} contained no state.");
                    return false;
                }

                foreach (var record in slotData.states)
                {
                    if (!_saveables.TryGetValue(record.id, out var saveable)) continue;
                    var stateType = Type.GetType(record.type);
                    if (stateType == null)
                    {
                        Debug.LogWarning($"Unknown state type {record.type} for id {record.id}.");
                        continue;
                    }

                    var state = JsonUtility.FromJson(record.json, stateType);
                    saveable.RestoreState(state);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load slot {slotName}: {ex}");
                return false;
            }
        }

        public bool HasSave(string slotName)
        {
            return File.Exists(GetSlotPath(slotName));
        }

        private void EnsureSaveDirectory()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

        private string GetSlotPath(string slotName)
        {
            return Path.Combine(SaveDirectory, $"{slotName}.json");
        }

        [Serializable]
        private class SaveSlotData
        {
            public string slotName;
            public string savedAtUtc;
            public List<StateRecord> states;
        }

        [Serializable]
        private class StateRecord
        {
            public string id;
            public string type;
            public string json;
        }
    }
}
