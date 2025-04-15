using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    /// <summary>
    /// Manages the three-deck system for cards: base deck, active deck, and discard pile.
    /// Handles shuffling, drawing cards, and moving cards between decks.
    /// </summary>
    public class DeckManager : MonoBehaviour
    {
        [Header("Deck Configuration")]
        [Tooltip("The player's permanent card collection. This is used to initialize the active deck at game start.")]
        [SerializeField] private List<Card> baseDeck = new List<Card>();

        /// <summary>
        /// Cards currently available to be drawn. This deck is depleted as cards are drawn.
        /// </summary>
        private List<Card> activeDeck = new List<Card>();

        /// <summary>
        /// Cards that have been used or discarded. These are shuffled back into the active deck when it runs empty.
        /// </summary>
        private List<Card> discardPile = new List<Card>();

        [Tooltip("Time in seconds to wait before automatically drawing a new card after one is used.")]
        [SerializeField] private float autoDrawDelay = 1f;
        [Tooltip("Marks if the Bullets will auto-draw")]
        [SerializeField] private bool willAutodraw = true;

        [Header("Dependencies")]
        [Tooltip("Reference to the CardManager that handles card slots and UI updates.")]
        [SerializeField] private CardManager cardManager;

        public CardManager CardManager => cardManager;

        private bool _isReloading = false;

        private void Awake()
        {
            if (cardManager) return;
            cardManager = GetComponent<CardManager>();
            if (!cardManager)
            {
                cardManager = this.GetComponentInScene(false, out cardManager);
            }

            if (cardManager) return;
            //Yeet play mode if we can't find a card manager
#if UNITY_EDITOR
            Debug.LogError("Failed to find card manager");
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        /// <summary>
        /// Initialize decks with a slight delay to ensure UI components are ready.
        /// </summary>
        private void Start()
        {
            // Initialize after a slight delay to ensure UI components are ready
            StartCoroutine(InitializeDecksDelayed());
        }

        /// <summary>
        /// Waits for the end of the frame before initializing decks to avoid UI race conditions.
        /// </summary>
        private IEnumerator InitializeDecksDelayed()
        {
            // Wait for end of frame to ensure UI components have initialized
            yield return new WaitForEndOfFrame();

            // Initialize decks
            InitializeDecks();
        }

        /// <summary>
        /// Sets up all three decks and loads card chambers at game start.
        /// Copies the base deck to the active deck, shuffles it, and fills all card slots.
        /// </summary>
        public void InitializeDecks()
        {
            // Clear all decks to start fresh
            activeDeck.Clear();
            discardPile.Clear();

            // Copy base deck to active deck
            activeDeck.AddRange(baseDeck);

            // Shuffle the active deck
            ShuffleDeck(activeDeck);

            // Load all chambers with cards
            for (int i = 0; i < cardManager.cardSlots.Length; i++)
            {
                DrawCardToSlot(i);
            }

            print($"Deck initialized with {activeDeck.Count} cards in active deck");
        }

        /// <summary>
        /// Draws a single card from the active deck.
        /// If the active deck is empty, shuffles the discard pile back into it first.
        /// </summary>
        /// <returns>The drawn card, or null if no cards are available.</returns>
        public Card DrawCard()
        {
            // If active deck is empty, shuffle discard pile into it
            if (activeDeck.Count == 0)
            {
                ShuffleDiscardIntoActiveDeck();
            }

            // If still empty (no cards at all), return null
            if (activeDeck.Count == 0)
            {
                print("No cards available in any deck!");
                return null;
            }

            // Draw top card from active deck
            Card drawnCard = activeDeck[0];
            activeDeck.RemoveAt(0);
            return drawnCard;
        }

        /// <summary>
        /// Draws a card from the active deck and places it in the specified slot.
        /// </summary>
        /// <param name="slotIndex">The slot index to place the card in (0-3).</param>
        public void DrawCardToSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= cardManager.cardSlots.Length) return;
            if (cardManager.cardSlots[slotIndex]) return; // Slot already filled

            Card drawnCard = DrawCard();
            if (drawnCard)
            {
                cardManager.SetCardInSlot(slotIndex, drawnCard);
            }
        }

        /// <summary>
        /// Adds a card to the discard pile.
        /// </summary>
        /// <param name="card">The card to discard.</param>
        public void DiscardCard(Card card)
        {
            if (card)
            {
                discardPile.Add(card);
            }
            GameContext.Instance.UIRevolverManager.ShowReloadIndicator = cardManager.IsChamberEmpty();
        }
        /// <summary>
        /// Reloads the Hand/Chambers if any is null.
        /// When reloading, disable card controls (use slots/reload).
        /// </summary>
        public void ReloadSlots()
        {
            if (willAutodraw && _isReloading) return;
            _isReloading = true;
            GameContext.Instance.UIRevolverManager.ShowReloadIndicator = false;
            GameContext.Instance.InputManager.ActivateCardInputs = false;
            int noReloadBullets = 1;
            for (int i = 0; i < cardManager.CardSlotsCount; i++)
            {
                if (cardManager.IsSlotEmpty(i))
                {

                    StartCoroutine(AutoDrawAfterDelay
                    (autoDrawDelay * noReloadBullets, i,
                    () =>
                    {
                        //If it's the last chamber to load, activate player controls.
                        if (cardManager.NoEmptySlots() <= 0)
                        {
                            GameContext.Instance.InputManager.ActivateCardInputs = true;
                            _isReloading = false;
                        }
                    }));
                    noReloadBullets++;
                }
            }
        }


        /// <summary>
        /// Starts a coroutine to draw a new card to the specified slot after a delay.
        /// </summary>
        /// <param name="slotIndex">The slot index to draw a card to (0-3).</param>
        public void StartAutoDrawForSlot(int slotIndex)
        {
            if (!willAutodraw) return;
            GameContext.Instance.UIRevolverManager.ShowReloadIndicator = false;
            StartCoroutine(AutoDrawAfterDelay(this.autoDrawDelay, slotIndex));
        }

        /// <summary>
        /// Waits for the specified delay time before drawing a card to the slot.
        /// </summary>
        /// <param name="slotIndex">The slot index to draw a card to (0-3).</param>
        private IEnumerator AutoDrawAfterDelay(float drawDelay, int slotIndex, Action optionalAction = null)
        {
            yield return new WaitForSeconds(drawDelay);
            DrawCardToSlot(slotIndex);
            optionalAction?.Invoke();
        }

        /// <summary>
        /// Randomizes the order of cards in the given deck using the Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <param name="deck">The deck to shuffle.</param>
        private void ShuffleDeck(List<Card> deck)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                Card temp = deck[i];
                int randomIndex = UnityEngine.Random.Range(i, deck.Count);
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Moves all cards from the discard pile to the active deck and shuffles them.
        /// Called when the active deck is empty and a card needs to be drawn.
        /// </summary>
        private void ShuffleDiscardIntoActiveDeck()
        {
            // Move all cards from discard to active deck
            activeDeck.AddRange(discardPile);
            discardPile.Clear();

            // Shuffle the active deck
            ShuffleDeck(activeDeck);

            print("Discard pile shuffled into active deck");
        }

        /// <summary>
        /// Returns the number of cards currently in the active deck.
        /// </summary>
        public int GetActiveCount() => activeDeck.Count;
        /// <summary>
        /// Returns the number of cards currently in the discard pile.
        /// </summary>
        public int GetDiscardCount() => discardPile.Count;
    }
}