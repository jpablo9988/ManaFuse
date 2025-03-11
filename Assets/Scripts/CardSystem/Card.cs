using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Card System/Card")]
    public sealed class Card : ScriptableObject {
        public string cardName;
        public Sprite cardIcon;
        public CardType cardType; //Grabs card type from enums below. Use for setting up card behavior
        //Add additional variables as required. 
        
        public void Activate(GameObject user) {
            //Implement what the card does here
            Debug.Log($"{cardName} activated!");
        }
    }

    public enum CardType {
        Attack,
        Heal,
        Dash
    }
}