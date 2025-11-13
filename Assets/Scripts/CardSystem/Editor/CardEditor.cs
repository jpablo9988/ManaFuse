using UnityEngine;
using UnityEditor;
using CardSystem;

namespace CardSystem.Editor
{
    /// <summary>
    /// Custom editor for the Card class that provides a dynamic inspector UI.
    /// Shows different fields based on the selected card type (Attack, Heal, or Dash).
    /// </summary>
    [CustomEditor(typeof(Card))]
    public class CardEditor : UnityEditor.Editor
    {
        // Serialized properties for basic card fields
        SerializedProperty cardName;
        SerializedProperty cardIcon;
        SerializedProperty cardType;
        SerializedProperty cardCost;
        SerializedProperty projectilePrefab;
        SerializedProperty particleEffectPrefab;
        SerializedProperty spawnProjectile;
        SerializedProperty spawnParticleEffect;
        
        // Serialized properties for type-specific fields
        SerializedProperty healAmount;
        SerializedProperty dashDistance;
        SerializedProperty dashDuration;
        
        /// <summary>
        /// Called when the editor is enabled or the inspected object changes.
        /// Finds all serialized properties for the Card class.
        /// </summary>
        void OnEnable()
        {
            // Find the serialized properties
            cardName = serializedObject.FindProperty("cardName");
            cardIcon = serializedObject.FindProperty("cardIcon");
            cardType = serializedObject.FindProperty("cardType");
            cardCost = serializedObject.FindProperty("cardCost");
            projectilePrefab = serializedObject.FindProperty("projectilePrefab");
            particleEffectPrefab = serializedObject.FindProperty("particleEffectPrefab");
            spawnProjectile = serializedObject.FindProperty("spawnProjectile");
            spawnParticleEffect = serializedObject.FindProperty("spawnParticleEffect");
            
            // Type-specific properties
            healAmount = serializedObject.FindProperty("healAmount");
            dashDistance = serializedObject.FindProperty("dashDistance");
            dashDuration = serializedObject.FindProperty("dashDuration");
        }
        
        /// <summary>
        /// Draws the custom inspector UI with type-specific fields.
        /// Called every time the inspector needs to be redrawn.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Update the serialized object to reflect changes
            serializedObject.Update();
            
            // Always show basic card properties
            EditorGUILayout.PropertyField(cardName);
            EditorGUILayout.PropertyField(cardIcon);
            EditorGUILayout.PropertyField(cardType);
            EditorGUILayout.PropertyField(cardCost);
            
            // Get the current card type
            CardType type = (CardType)cardType.enumValueIndex;
            
            // Display type-specific fields based on the card type
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Type-Specific Settings", EditorStyles.boldLabel);
            
            switch (type)
            {
                case CardType.Attack:
                    // Attack cards show projectile settings
                    EditorGUILayout.PropertyField(projectilePrefab);
                    EditorGUILayout.PropertyField(spawnProjectile);
                    break;
                    
                case CardType.Heal:
                    // Heal cards show healing amount
                    EditorGUILayout.PropertyField(healAmount);
                    break;
                    
                case CardType.Dash:
                    // Dash cards show distance and duration
                    EditorGUILayout.PropertyField(dashDistance);
                    EditorGUILayout.PropertyField(dashDuration);
                    break;
            }
            
            // Visual effects for all types
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Visual Effects", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(particleEffectPrefab);
            EditorGUILayout.PropertyField(spawnParticleEffect);
            
            // Apply changes to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
} 