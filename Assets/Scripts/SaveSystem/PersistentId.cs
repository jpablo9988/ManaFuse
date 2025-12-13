using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Ensures the GameObject has a stable GUID that can be referenced from save data.
    /// Generates ids both in edit and play mode and warns about duplicates in the active scene.
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public sealed class PersistentId : MonoBehaviour
    {
        [SerializeField] private string id;
        private static readonly Dictionary<string, PersistentId> Lookup = new();

        public string Id => id;

        private void Awake()
        {
            if (Application.isPlaying)
            {
                RegisterId();
            }
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                UnregisterId();
            }
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString("N");
            }
        }

        private void RegisterId()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString("N");
            }

            if (Lookup.TryGetValue(id, out var existing) && existing != null && existing != this)
            {
                Debug.LogWarning($"Duplicate PersistentId detected on {name}. Regenerating id.");
                id = Guid.NewGuid().ToString("N");
            }

            Lookup[id] = this;
        }

        private void UnregisterId()
        {
            if (!string.IsNullOrEmpty(id) && Lookup.TryGetValue(id, out var existing) && existing == this)
            {
                Lookup.Remove(id);
            }
        }
    }
}
