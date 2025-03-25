using UnityEngine;

namespace CardSystem
{
    public class CardEffectHandler : MonoBehaviour {

        //Executes a method based on what type of card it is
        public void ExecuteCardEffect(Card cardData, GameObject user) {
            switch (cardData.cardType) {
                case CardType.Attack:
                    ExecuteAttack(cardData, user);
                    break;
                case CardType.Heal:
                    ExecuteHeal(cardData, user);
                    break;
                case CardType.Movement:
                    ExecuteMovement(cardData, user);
                    break;
                default:
                    Debug.LogWarning("Card type not handled!");
                    break;
            }
        }

        private void ExecuteAttack(Card cardData, GameObject user) {
            Debug.Log($"{cardData.cardName} activated: dealing {cardData.damage} damage.");
            GameContext.Instance.UIManafuseBar.ChangeByUnit(-cardData.cardCost, false);
            GameContext.Instance.ProjectileManager.ShootProjectile();

        }

        private void ExecuteHeal(Card cardData, GameObject user) {
            Debug.Log($"{cardData.cardName} activated: healing {cardData.healAmount} HP.");
        }

        private void ExecuteMovement(Card cardData, GameObject user) {
            Debug.Log($"{cardData.cardName} activated: performing movements ability.");
        }
    }
}