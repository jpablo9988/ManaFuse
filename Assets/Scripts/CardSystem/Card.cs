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
            GameContext.Instance.Player.ChangeMana(-cardCost, false);
            //TODO: For now, each card is shooting a basic projectile. Change this according to the funcionality.
            GameContext.Instance.ProjectileManager.ShootProjectile();
        }
    }

    public enum CardType
    {
        Attack,
        Heal,
        Dash
    }
}