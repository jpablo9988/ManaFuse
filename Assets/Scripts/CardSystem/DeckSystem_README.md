# Deck System Implementation Guide

This document provides instructions for setting up the new deck system in Unity.

## System Overview

The new deck system consists of three decks:
1. **Base Deck**: The player's permanent card collection
2. **Active Deck**: Cards currently available to be drawn
3. **Discard Pile**: Cards that have been used or discarded

When a card is activated, it's moved to the discard pile. When the active deck is empty, the discard pile is shuffled into the active deck.

## Unity Setup

1. **Create the DeckManager Component**:
   - Add the `DeckManager` component to the same GameObject as your `CardManager`
   - In the Inspector, populate the "Base Deck" list with your card assets
   - Set the "Auto Draw Delay" to define how quickly new cards are drawn after using one (default: 1 second)
   - Connect the CardManager reference 

2. **Update GameContext**:
   - Make sure the GameContext singleton has a reference to the DeckManager
   - The code has been updated to find and create the DeckManager if missing

3. **Remove Previous Deck Setup**:
   - The player deck is now managed by the DeckManager
   - Remove any previous code that manually loaded cards into chambers
   - The system will automatically initialize cards in chambers on startup

## Testing

1. Check the Inspector to see how many cards are in each deck:
   - `DeckManager.GetActiveCount()`: Number of cards in the active deck
   - `DeckManager.GetDiscardCount()`: Number of cards in the discard pile

2. Verify that:
   - The chambers automatically load on game start
   - Used cards go to the discard pile
   - New cards are automatically drawn after a delay
   - When the active deck is empty, the discard pile shuffles in

## Important Notes

- Make sure all your card assets are included in the Base Deck list
- The CardInputHandler has been simplified to remove manual card drawing
- The system works with existing RevolverManagerUI and BulletManagerUI components without modification 