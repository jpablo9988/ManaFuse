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
    // [SerializeField]
    // private string newTag = "";
    // [SerializeField]
    // [Range(0, 99)]
    // private int newLayer = 0;
    // [SerializeField]
    // private Material mainTexture;
    // [SerializeField]
    // private List<Material> textureVariations;
    // [SerializeField]
    // [Range(0, 100)]
    // private int variationChance;
    // [Header("Object Prefab Reference")]
    // [SerializeField]
    // private GameObject prefab;
    // [SerializeField]
    // private GameObject parentPrefab;
    [Header("Generation Settings")]
    [SerializeField]
    private CubeGenerationType genType;
    private Vector3 prefabSize;
    [Header("Face Generation Attributes")]
    public bool generateFloor = true;
    public bool generateCeiling = true;
    public bool generateFrontX = true;
    public bool generateBackX = true;
    public bool generateFrontZ = true;
    public bool generateBackZ = true;
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
        get
        {
            if (genType == null) return null;
            return genType.mainTexture;
        }
    }
    public GameObject Prefab
    {
        private set { }
        get
        {
            if (genType == null) return null;
            return genType.prefab;
        }
    }
    public GameObject ParentPrefab
    {
        private set { }
        get
        {
            if (genType == null) return null;
            return genType.parentPrefab;
        }
    }
    public void GenerateCubes()
    {
        prefabSize = Prefab.GetComponent<MeshRenderer>().bounds.size;
        GameObject parentGO = Instantiate(ParentPrefab, this.transform.position, Quaternion.Euler(startingRotation));
        parentGO.name = parentName;
        if (genType.newTag != "")
        {
            parentGO.tag = genType.newTag;
        }
        parentGO.layer = genType.newLayer;
        //High Complexity. It's only gonna run in the editor tho. Probably a better way to write this...
        for (int x = 0; x < Mathf.Abs(dimensions.x); x++)
        {
            for (int y = 0; y < Mathf.Abs(dimensions.y); y++)
            {
                for (int z = 0; z < Mathf.Abs(dimensions.z); z++)
                {
                    if (DoesBlockFollowConditionals(x, y, z))
                    {
                        //TODO: Do more conditional instantiations based on generation bool prefs.
                        Vector3 prefabPos = new(prefabSize.x * x, prefabSize.y * y, prefabSize.z * z);
                        Material auxMaterial;
                        int willUseMainTexture = Random.Range(0, 100);
                        if (willUseMainTexture < genType.variationChance || genType.textureVariations.Count == 0)
                        {
                            auxMaterial = genType.mainTexture;
                        }
                        else
                        {
                            int indexOfVariation = Random.Range(0, genType.textureVariations.Count);
                            auxMaterial = genType.textureVariations[indexOfVariation];
                        }
                        GameObject auxGo = Instantiate(genType.prefab, parentGO.transform, false);
                        auxGo.transform.localPosition = prefabPos;
                        if (genType.newTag != "")
                        {
                            auxGo.tag = genType.newTag;
                        }
                        auxGo.layer = genType.newLayer;
                        auxGo.GetComponent<Renderer>().material = auxMaterial;
                        Debug.Log(prefabPos);
                    }
                }
            }
        }
    }
    // If any of the conditionals (block being on the outside planes) are true, returns true.
    // Simplifies syntax on main function because it's a big conditional.
    /*
    private bool IsBlockOnOutsidePlanes(int x, int y, int z)
    {
        return x == 0 || x == Mathf.Abs(dimensions.x) - 1 ||
        y == 0 || y == Mathf.Abs(dimensions.y) - 1 ||
        z == 0 || x == Mathf.Abs(dimensions.z) - 1;
    }*/
    private bool DoesBlockFollowConditionals(int x, int y, int z)
    {
        return (generateFloor && y == 0) || (generateCeiling && y == Mathf.Abs(dimensions.y) - 1) ||
        (generateBackX && x == 0) || (generateFrontX && x == Mathf.Abs(dimensions.x) - 1)
        || (generateBackZ && z == 0) || (generateFrontZ && z == Mathf.Abs(dimensions.z) - 1);

    }

}
