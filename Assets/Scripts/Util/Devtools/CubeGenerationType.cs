using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGenType", menuName = "DevTools/Generation/Generation Type")]

public class CubeGenerationType : ScriptableObject
{
    public string newTag = "";
    [Range(0, 99)]
    public int newLayer = 0;
    public Material mainTexture;
    public List<Material> textureVariations;
    [Range(0, 99)]
    public int variationChance;
    [Header("Object Prefab Reference")]
    public GameObject prefab;
    public GameObject parentPrefab;
}
