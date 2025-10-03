using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    [Header("Generation Attrbiutes")]
    [SerializeField]
    private Vector3 dimensions;
    [SerializeField]
    private Material mainTexture;
    [SerializeField]
    private List<Material> textureVariations;
    [SerializeField]
    [Range(0f, 1f)]
    private float variationChance;
    [SerializeField]
    private string cubeTag;
    [SerializeField]
    private string layer;
    [Header("Object Prefab Reference")]
    [SerializeField]
    private GameObject prefab;
    private Vector3 prefabSize;
    public Vector3 Dimensions
    {
        private set { }
        get { return dimensions; }
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
    public void GenerateCubes()
    {
        prefabSize = prefab.GetComponent<MeshRenderer>().bounds.size;
        for (int x = 0; x < Mathf.Abs(dimensions.x); x++)
        {
            Vector3 prefabPos = new(prefabSize.x * x, 0, 0);
            Debug.Log(prefabPos);
            for (int y = 0; y < Mathf.Abs(dimensions.y); y++)
            {
                prefabPos = new(prefabSize.x * x, prefabSize.y * y, 0);
                Debug.Log(prefabPos);
                for (int z = 0; z < Mathf.Abs(dimensions.z); z++)
                {
                    prefabPos = new(prefabSize.x * x, prefabSize.y * y, prefabSize.z * z);
                    Debug.Log(prefabPos);
                }
            }
        }
    }

}
