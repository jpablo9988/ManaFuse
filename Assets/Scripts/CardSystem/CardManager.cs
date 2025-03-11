using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardSystem
{
    public class CardManager : MonoBehaviour
    {
        //PlayerDeck
        public List<Card> playerDeck = new List<Card>();

        //Slot allocation
        public Card[] cardSlots = new Card[4];

        //Draw card into defined slot
        public void DrawCardToSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= cardSlots.Length) return;
            // For example, randomly choose a card from allCards:
            if (cardSlots[slotIndex] != null) return; //There is already a card in this position, do not add.
            var drawnCard = playerDeck[Random.Range(0, playerDeck.Count)];
            cardSlots[slotIndex] = drawnCard;
            UpdateSlotUI(slotIndex, drawnCard);
        }

        //Discard the card from the slot
        public void DiscardCardFromSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= cardSlots.Length) return;
            cardSlots[slotIndex] = null;
            UpdateSlotUI(slotIndex);
        }

        //Activate the card in the slot
        public void ActivateCardInSlot(int slotIndex, GameObject user)
        {
            if (slotIndex < 0 || slotIndex >= cardSlots.Length) return;
            var card = cardSlots[slotIndex];
            if (card != null)
            {
                card.Activate(user);
            }
            DiscardCardFromSlot(slotIndex);
        }


        private static void UpdateSlotUI(int slotIndex, Card card = null)
        {
            if (card == null)
            {
                GameContext.Instance.UIRevolverManager.DiscardCard((BulletDirection)slotIndex);
                return;
            }
            GameContext.Instance.UIRevolverManager.LoadCard((BulletDirection)slotIndex, card);
            Debug.Log($"Slot {slotIndex} updated.");
        }
    }
}