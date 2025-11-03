using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class CubeGenerator : MonoBehaviour
{
    [Header("Generation Attrbiutes")]
    [SerializeField]
    private string parentName;
    [SerializeField]
    private Vector3 dimensions;
    [SerializeField]
    private Vector3 startingRotation;
    [SerializeField]
    private string newTag = "";
    [SerializeField]
    [Range(0, 99)]
    private int newLayer = 0;
    [SerializeField]
    private Material mainTexture;
    [SerializeField]
    private List<Material> textureVariations;
    [SerializeField]
    [Range(0, 100)]
    private int variationChance;
    [Header("Object Prefab Reference")]
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GameObject parentPrefab;
    private Vector3 prefabSize;
    public Vector3 Dimensions
    {
        private set { }
        get { return dimensions; }
    }
    public Vector3 StartingRotation
    {
        private set { }
        get { return startingRotation; }
    }
    public Material MainTexture
    {
        private set { }
        get { return mainTexture; }
    }
    public GameObject Prefab
    {
        private set { }
        get { return prefab; }
    }
    public GameObject ParentPrefab
    {
        private set { }
        get { return parentPrefab; }
    }
    public void GenerateCubes()
    {
        prefabSize = prefab.GetComponent<MeshRenderer>().bounds.size;
        GameObject parentGO = Instantiate(parentPrefab, this.transform.position, Quaternion.Euler(startingRotation));
        parentGO.name = parentName;
        if (newTag != "")
        {
            parentGO.tag = newTag;
        }
        parentGO.layer = newLayer;
        //High Complexity. It's only gonna run in the editor tho. Probably a better way to write this...
        for (int x = 0; x < Mathf.Abs(dimensions.x); x++)
        {
            for (int y = 0; y < Mathf.Abs(dimensions.y); y++)
            {
                for (int z = 0; z < Mathf.Abs(dimensions.z); z++)
                {
                    Vector3 prefabPos = new(prefabSize.x * x, prefabSize.y * y, prefabSize.z * z);
                    Material auxMaterial;
                    int willUseMainTexture = Random.Range(0, 100);
                    if (willUseMainTexture < variationChance || textureVariations.Count == 0)
                    {
                        auxMaterial = mainTexture;
                    }
                    else
                    {
                        int indexOfVariation = Random.Range(0, textureVariations.Count);
                        auxMaterial = textureVariations[indexOfVariation];
                    }
                    GameObject auxGo = Instantiate(prefab, parentGO.transform, false);
                    auxGo.transform.localPosition = prefabPos;
                    if (newTag != "")
                    {
                        auxGo.tag = newTag;
                    }
                    auxGo.layer = newLayer;
                    auxGo.GetComponent<Renderer>().material = auxMaterial;
                    Debug.Log(prefabPos);
                }
            }
        }
    }

}
