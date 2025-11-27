using System;
using System.Collections.Generic;
using System.Linq;
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

        public int CardSlotsCount => cardSlots.Length;

        /// <summary>
        /// Get references to required components on initialization.
        /// </summary>
        private void Awake()
        {
            if (deckManager) return;
            deckManager = GetComponent<DeckManager>();
            if (!deckManager)
            {
                deckManager = this.GetComponentInScene(false, out deckManager);
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
            //Card is listening:
            if (card != null && card.cardType == CardType.Attack)
            {
                Projectile.OnHitEnemy += card.OnHitTarget;
            }
            if (card)
            {
                print($"Setting {card.cardName} in slot {slotIndex}");
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
        private void UpdateSlotUI(int slotIndex, Card card = null)
        {
            if (!GameContext.Instance.UIRevolverManager)
            {
                Debug.LogWarning("UIRevolverManager not found in GameContext");
                return;
            }

            if (!card)
            {
                GameContext.Instance.UIRevolverManager.DiscardCard((BulletDirection)slotIndex);
                return;
            }

            GameContext.Instance.UIRevolverManager.LoadCard((BulletDirection)slotIndex, card);
            print($"Slot {slotIndex} updated with {card.cardName}");
        }
        /// <summary>
        /// Will iterate over the card list and check if all items are null or not.
        /// </summary>
        /// <param name="_actionIfEmptyMarkFirst"> Executes an action if the chamber has no bullets,
        /// passing a bool indicating if it's the first slot checked.</param>
        /// <param name="_actionIfEmpty">Ex</param> Executes an action if the chamber has no bullets.
        /// <returns></returns>
        public bool IsChamberEmpty(int startIndex = -1)
        {
            for (int i = startIndex + 1; i < cardSlots.Length; i++)
            {
                if (cardSlots[i] != null) return false;
            }
            return true;
        }
        /// <summary>
        /// Checks if the specific slot is empty (card is null).
        /// If the index is outside of the boundaries, always returns true.
        /// </summary>
        /// <param name="index"> Index of the Element to check</param>
        /// <returns></returns>
        public bool IsSlotEmpty(int index)
        {
            if (index < 0 || index >= cardSlots.Length) return true;
            return cardSlots[index] == null;
        }
        /// <summary>
        /// Returns the amount of slots that holds a null card (or empty card)
        /// </summary>
        /// <returns></returns>
        public int NoEmptySlots()
        {
            return cardSlots.Count(card => card == null);
        }
    }
}