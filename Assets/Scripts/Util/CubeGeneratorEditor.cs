using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CubeGenerator), true)]
[CanEditMultipleObjects]
public class CubeGeneratorEditor : Editor
{
    SerializedProperty parentName, newLayer, newTag, dimensions, startingRotation, mainTexture, textureVariations, variationChance, prefab, parentPrefab;
    void OnEnable()
    {
        parentName = serializedObject.FindProperty("parentName");
        newLayer = serializedObject.FindProperty("newLayer");
        newTag = serializedObject.FindProperty("newTag");
        dimensions = serializedObject.FindProperty("dimensions");
        startingRotation = serializedObject.FindProperty("startingRotation");
        mainTexture = serializedObject.FindProperty("mainTexture");
        textureVariations = serializedObject.FindProperty("textureVariations");
        variationChance = serializedObject.FindProperty("variationChance");
        prefab = serializedObject.FindProperty("prefab");
        parentPrefab = serializedObject.FindProperty("parentPrefab");
    }
    public override void OnInspectorGUI()
    {
        CubeGenerator cubeGenerator = (CubeGenerator)target;
        EditorGUILayout.PropertyField(parentName, new GUIContent("Parent Name"));
        EditorGUILayout.PropertyField(dimensions, new GUIContent("Dimensions"));
        EditorGUILayout.PropertyField(startingRotation, new GUIContent("Starting Rotation"));
        EditorGUILayout.PropertyField(newTag, new GUIContent("Tag"));
        EditorGUILayout.PropertyField(newLayer, new GUIContent("Layer (Number)"));
        EditorGUILayout.PropertyField(mainTexture, new GUIContent("Main Texture"));
        if (cubeGenerator.MainTexture != null)
        {
            EditorGUILayout.PropertyField(textureVariations, new GUIContent("Texture Variations"));
            EditorGUILayout.PropertyField(variationChance, new GUIContent("Variation Chance"));
        }
        EditorGUILayout.PropertyField(prefab, new GUIContent("Target Prefab"));
        EditorGUILayout.PropertyField(parentPrefab, new GUIContent("Parent Prefab"));
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
