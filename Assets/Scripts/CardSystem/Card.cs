using UnityEditor.Rendering;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CardSystem
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Card System/Card")]
    public sealed class Card : ScriptableObject
    {
        public string cardName;
        public Sprite cardIcon;
        public CardType cardType;
        public int cardCost;

        //Attack card vars
        public float damage;
        public float attackRadius;
        public DamageType damageType;


        //Heal card vars
        public float healAmount;

        //Movements vars

        //Attack & Movement vars
        public float range;

        #if UNITY_EDITOR //Check if we are within the unity editor

        [CustomEditor(typeof(Card))] //Custom card editor that lets us show only relevant options for the cards :)
        private class CardEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                //These will be the common fields for our cards
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cardName"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cardIcon"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cardType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cardCost"));

                //Get our card type
                CardType currentType = (CardType)serializedObject.FindProperty("cardType").enumValueIndex;

                //Display only fields relevant to the card we are creating.
                switch (currentType)
                {
                    case CardType.Attack:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRadius"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("damageType"));

                        break;
                    case CardType.Heal:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("healAmount"));
                        break;
                    case CardType.Movement:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
                        break;
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
#endif
    }

    public enum CardType
    {
        Attack,
        Heal,
        Movement
    }
    public enum DamageType
    {
        Basic,
        Poison,
        Fire,
        Curse,
        Acid,
        Healing,
        Ice
    }
}
