using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardSystem
{
    public class CardManager : MonoBehaviour {
        //PlayerDeck
        public List<Card> playerDeck = new List<Card>();

        //Slot allocation
        public Card[] cardSlots = new Card[4];

        //Draw card into defined slot
        public void DrawCardToSlot(int slotIndex) {
            if (slotIndex < 0 || slotIndex >= cardSlots.Length) return;
            // For example, randomly choose a card from allCards:
            var drawnCard = playerDeck[Random.Range(0, playerDeck.Count)];
            cardSlots[slotIndex] = drawnCard;
            UpdateSlotUI(slotIndex);
        }

        //Discard the card from the slot
        public void DiscardCardFromSlot(int slotIndex) {
            if (slotIndex < 0 || slotIndex >= cardSlots.Length) return;
            cardSlots[slotIndex] = null;
            UpdateSlotUI(slotIndex);
        }

        //Activate the card in the slot
        public void ActivateCardInSlot(int slotIndex, GameObject user) {
            if (slotIndex < 0 || slotIndex >= cardSlots.Length) return;
            var card = cardSlots[slotIndex];
            if (card != null) {
                card.Activate(user);
            }
        }

        
        private static void UpdateSlotUI(int slotIndex) {
            //UI Stuff skeleton
            Debug.Log($"Slot {slotIndex} updated.");
        }
    }
}