using System.Collections.Generic;
using CardSystem;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialEvents : MonoBehaviour
{
    public Card basicCard;
    public void AddRemainingBulletsToDeck()
    {
        //Temporary bullets to add to deck. It's not persistent.
        GameContext.Instance.DeckManager.AddTemporaryCardToDeck(
            new List<Card>()
            {
                basicCard,
                basicCard,
                basicCard
            }
        );
    }
    public void StartTimer()
    {
        GameContext.Instance.Player.IsTimerActive = true;
    }
}
