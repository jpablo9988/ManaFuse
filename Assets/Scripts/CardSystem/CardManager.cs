using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardSystem
{
    /// <summary>
    /// Manages the card slots/chambers and handles interactions between cards, UI, and the deck system.
    /// Works with DeckManager to handle drawing, activating, and discarding cards.
    /// </summary>
    public class CardManager : MonoBehaviour
    {
        [Header("Slot allocation")]
        [Tooltip("The four card slots/chambers that hold cards ready to be activated. Maps to NORTH, EAST, SOUTH, WEST directions.")]
        public Card[] cardSlots = new Card[4];

        [Header("Dependencies")]
        [Tooltip("Reference to the DeckManager that manages the card decks and drawing.")]
        [SerializeField] private DeckManager deckManager;

        /// <summary>
        /// Get references to required components on initialization.
        /// </summary>
        private void Awake()
        {
            if (deckManager == null)
            {
                deckManager = GetComponent<DeckManager>();
                if (deckManager == null)
                {
                    deckManager = this.GetComponentInScene(false, out deckManager);
                }
            }
        }

        /// <summary>
        /// Places a card in a specific slot and updates the UI to show the card.
        /// Used by DeckManager when drawing cards to slots.
        /// </summary>
        /// <param name="slotIndex">The slot index to set the card in (0-3).</param>
        /// <param name="card">The card to place in the slot.</param>
        public void SetCardInSlot(int slotIndex, Card card)
        {
            if (slotIndex < 0 || slotIndex >= cardSlots.Length) return;
            cardSlots[slotIndex] = card;

            if (card != null)
            {
                Debug.Log($"Setting {card.cardName} in slot {slotIndex}");
            }

            UpdateSlotUI(slotIndex, card);
        }

        /// <summary>
        /// Activates the card in the specified slot.
        /// Removes the card from the slot, activates its effect, adds it to the discard pile,
        /// and schedules a new card to be drawn after a delay.
        /// </summary>
        /// <param name="slotIndex">The slot index to activate (0-3).</param>
        /// <param name="user">The GameObject that is using the card (typically the player).</param>
        public void ActivateCardInSlot(int slotIndex, GameObject user)
        {
            if (slotIndex < 0 || slotIndex >= cardSlots.Length) return;
            var card = cardSlots[slotIndex];
            if (card != null)
            {
                // Store the card before removing it from the slot
                Card usedCard = card;

                // Remove from slot
                cardSlots[slotIndex] = null;
                UpdateSlotUI(slotIndex);

                // Activate the card
                usedCard.Activate(user);

                // Add to discard pile
                deckManager.DiscardCard(usedCard);

                // Start auto-draw for this slot
                deckManager.StartAutoDrawForSlot(slotIndex);
            }
        }

        /// <summary>
        /// Manually discards the card from the specified slot.
        /// Removes the card, updates the UI, and adds the card to the discard pile.
        /// Does not automatically draw a new card.
        /// </summary>
        /// <param name="slotIndex">The slot index to discard from (0-3).</param>
        public void DiscardCardFromSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= cardSlots.Length) return;

            // Get the card before removing it
            Card discardedCard = cardSlots[slotIndex];

            // Remove from slot
            cardSlots[slotIndex] = null;
            UpdateSlotUI(slotIndex);

            // Add to discard pile if it exists
            if (discardedCard != null)
            {
                deckManager.DiscardCard(discardedCard);
            }
        }

        /// <summary>
        /// Updates the UI to show or hide a card in a specific slot.
        /// Uses the RevolverManagerUI to update the visual representation.
        /// </summary>
        /// <param name="slotIndex">The slot index to update (0-3).</param>
        /// <param name="card">The card to display, or null to hide the slot.</param>
        private static void UpdateSlotUI(int slotIndex, Card card = null)
        {
            if (GameContext.Instance?.UIRevolverManager == null)
            {
                Debug.LogWarning("UIRevolverManager not found in GameContext");
                return;
            }

            if (card == null)
            {
                GameContext.Instance.UIRevolverManager.DiscardCard((BulletDirection)slotIndex);
                return;
            }

            GameContext.Instance.UIRevolverManager.LoadCard((BulletDirection)slotIndex, card);
            Debug.Log($"Slot {slotIndex} updated with {card.cardName}");
        }
    }
}