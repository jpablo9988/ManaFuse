using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int TotalEnemies { get; private set; }
    private bool isGameOver = false;

    public bool IsGameOver => isGameOver;

    public void RegisterEnemy()
    {
        TotalEnemies++;
        Debug.Log($"[GameStateManager] Enemy registered. Total: {TotalEnemies}");
    }

    public void UnregisterEnemy()
    {
        if (isGameOver) return;

        TotalEnemies--;
        Debug.Log($"[GameStateManager] Enemy unregistered. Remaining: {TotalEnemies}");

        if (TotalEnemies <= 0)
            CheckVictoryCondition();
    }

    public void PlayerDied()
    {
        if (isGameOver) return;

        isGameOver = true;
        ShowGameOver();
    }

    private void CheckVictoryCondition()
    {
        if (isGameOver) return;

        isGameOver = true;
        ShowVictory();
    }

    private void ShowVictory()
    {
        Debug.Log("Open Victory Scene");
        SceneTransitionManager.Instance.LoadScene(SceneIndex.Victory);
    }

    private void ShowGameOver()
    {
        Debug.Log("Open Gameover Scene");
        SceneTransitionManager.Instance.LoadScene(SceneIndex.GameOver);
    }
}
