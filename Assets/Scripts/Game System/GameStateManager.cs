using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Track the number of enemies in the current scene
    public int TotalEnemies { get; private set; }

    // Track if the game is over (either victory or game )
    private bool isGameOver = false;

    // This property indicates if the game is over
    public bool IsGameOver => isGameOver;

    // This method is called when an enemy is spawned
    public void RegisterEnemy()
    {
        TotalEnemies++;
        Debug.Log($"[GameStateManager] Enemy registered. Total: {TotalEnemies}");
    }

    // Remove the enemy from the total count(enemy dead)
    public void UnregisterEnemy()
    {
        if (isGameOver) return;

        TotalEnemies--;
        Debug.Log($"[GameStateManager] Enemy unregistered. Remaining: {TotalEnemies}");

        if (TotalEnemies <= 0)
        {
            CheckVictoryCondition();
        }
    }

    // Called when the player dies
    public void PlayerDied()
    {
        if (isGameOver) return;

        isGameOver = true;
        ShowGameOver();
    }

    // Check if win (all enemies are defeated)
    private void CheckVictoryCondition()
    {
        if (isGameOver) return;

        isGameOver = true;
        ShowVictory();
    }

    // Show victory message (can be replaced with UI or scene switch)
    private void ShowVictory()
    {       
        Debug.Log("Open Victory Scene");
        // TODO:change to actual victory UI or scene switch
    }

    // Show game over message (can be replaced with UI or scene switch)
    private void ShowGameOver()
    {
        Debug.Log("Open Gameover Scene");
        // TODO: change to actual game over UI or scene switch
    }
}
