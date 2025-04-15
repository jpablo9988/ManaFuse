using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    // Add your UI elements here
    public void OnStartGame()
    {
        SceneTransitionManager.Instance.LoadScene(SceneIndex.PrototypeScene);
    }

    public void OnOpenSettings()
    {
        SceneTransitionManager.Instance.LoadScene(SceneIndex.Setting);
    }

    public void OnBackToMainMenu()
    {
        SceneTransitionManager.Instance.LoadScene(SceneIndex.Main);
    }

    public void OnGameOver()
    {
        SceneTransitionManager.Instance.LoadScene(SceneIndex.GameOver);
    }

    public void OnVictory()
    {
        SceneTransitionManager.Instance.LoadScene(SceneIndex.Victory);
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
