using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CubeGenerator), true)]
[CanEditMultipleObjects]
public class CubeGeneratorEditor : Editor
{
    SerializedProperty parentName, dimensions, startingRotation, genType;
    SerializedProperty generateFloor, generateCeiling, generateFrontX, generateBackX, generateFrontZ, generateBackZ;
    void OnEnable()
    {
        parentName = serializedObject.FindProperty("parentName");
        dimensions = serializedObject.FindProperty("dimensions");
        startingRotation = serializedObject.FindProperty("startingRotation");
        genType = serializedObject.FindProperty("genType");
        generateFloor = serializedObject.FindProperty("generateFloor");
        generateCeiling = serializedObject.FindProperty("generateCeiling");
        generateFrontX = serializedObject.FindProperty("generateFrontX");
        generateBackX = serializedObject.FindProperty("generateBackX");
        generateFrontZ = serializedObject.FindProperty("generateFrontZ");
        generateBackZ = serializedObject.FindProperty("generateBackZ");

    }
    public override void OnInspectorGUI()
    {
        CubeGenerator cubeGenerator = (CubeGenerator)target;
        EditorGUILayout.PropertyField(parentName, new GUIContent("Parent Name"));
        EditorGUILayout.PropertyField(dimensions, new GUIContent("Dimensions"));
        EditorGUILayout.PropertyField(startingRotation, new GUIContent("Starting Rotation"));
        EditorGUILayout.PropertyField(genType, new GUIContent("Generation Type"));
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
        EditorGUILayout.PropertyField(generateFloor, new GUIContent("Will Generate Floor(y)?"));
        EditorGUILayout.PropertyField(generateCeiling, new GUIContent("Will Generate Ceiling(y)?"));
        EditorGUILayout.PropertyField(generateFrontX, new GUIContent("Will Generate Front(X)?"));
        EditorGUILayout.PropertyField(generateBackX, new GUIContent("Will Generate Back(X)?"));
        EditorGUILayout.PropertyField(generateFrontZ, new GUIContent("Will Generate Front(Z)?"));
        EditorGUILayout.PropertyField(generateBackZ, new GUIContent("Will Generate Back(Z)?"));
        serializedObject.ApplyModifiedProperties();
    }
    private bool CanGenerate(CubeGenerator gen)
    {
        return Vector3.Magnitude(gen.Dimensions) > 0 && gen.Prefab != null;
    }
}
