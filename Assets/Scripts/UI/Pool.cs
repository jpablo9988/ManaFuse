using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> pooledObjects;
    void Awake()
    {
        pooledObjects = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            pooledObjects.Add(transform.GetChild(i).gameObject);
        }
    }

    public GameObject GetInactiveObject()
    {
        return pooledObjects.Where(pooledObject => pooledObject.activeInHierarchy == false).FirstOrDefault();
    }
    public void SetAllObjectsInactive()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (obj.activeSelf) obj.SetActive(false);
        }
    }
}
