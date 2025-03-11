using UnityEngine;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "NewCard", menuName = "Card System/Card")]
    public sealed class Card : ScriptableObject
    {
        public string cardName;
        public Sprite cardIcon;
        public CardType cardType; //Grabs card type from enums below. Use for setting up card behavior
        public int cardCost;

        public void Activate(GameObject user)
        {
            //Implement what the card does here
            Debug.Log($"{cardName} activated!");
            //Update UI
            //TODO: All of this is ideally done with an event. Refactor code whenever possible:
            GameContext.Instance.UIManafuseBar.ChangeByUnit(-cardCost, false);
        }
    }

    public enum CardType
    {
        Attack,
        Heal,
        Dash
    }
}