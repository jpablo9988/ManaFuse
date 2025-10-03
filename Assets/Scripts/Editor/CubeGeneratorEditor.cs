using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CubeGenerator), true)]
[CanEditMultipleObjects]
public class CubeGeneratorEditor : Editor
{
    SerializedProperty dimensions, mainTexture, textureVariations, variationChance, cubeTag, prefab, layer;
    void OnEnable()
    {
        dimensions = serializedObject.FindProperty("dimensions");
        mainTexture = serializedObject.FindProperty("mainTexture");
        textureVariations = serializedObject.FindProperty("textureVariations");
        variationChance = serializedObject.FindProperty("variationChance");
        cubeTag = serializedObject.FindProperty("cubeTag");
        layer = serializedObject.FindProperty("layer");
        prefab = serializedObject.FindProperty("prefab");
    }
    public override void OnInspectorGUI()
    {
        CubeGenerator cubeGenerator = (CubeGenerator)target;
        EditorGUILayout.PropertyField(dimensions, new GUIContent("Dimensions"));
        EditorGUILayout.PropertyField(mainTexture, new GUIContent("Main Texture"));
        if (cubeGenerator.MainTexture != null)
        {
            EditorGUILayout.PropertyField(textureVariations, new GUIContent("Texture Variations"));
            EditorGUILayout.PropertyField(variationChance, new GUIContent("Variation Chance"));
        }
        EditorGUILayout.PropertyField(cubeTag, new GUIContent("Instantiation Tag"));
        EditorGUILayout.PropertyField(layer, new GUIContent("Layer Type"));
        EditorGUILayout.PropertyField(prefab, new GUIContent("Target Prefab"));

        if (GUILayout.Button("Generate Block"))
        {
            if (CanGenerate(cubeGenerator))
            {
                cubeGenerator.GenerateCubes();
            }
            else
            {
                EditorUtility.DisplayDialog("Mistake-aroo!", "Please assign a Prefab to the Generator and Assign Valid Dimensions...!", "Gotcha.");
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
    private bool CanGenerate(CubeGenerator gen)
    {
        return Vector3.Magnitude(gen.Dimensions) > 0 && gen.Prefab != null;
    }
}
