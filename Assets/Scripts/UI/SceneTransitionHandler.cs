using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField]
    private Scene _fallbackScene = Scene.MAIN_MENU; // Scene to load if next level doesn't exist

    /// <summary>
    /// Very Placeholer. Loads One scene as single. Closes other screens.
    /// Doesn't support loading screens.
    /// </summary>
    /// <param name="scene">Scene to Go.</param>
    public void GoToScene(SceneTransitionVote sceneVote)
    {
        SceneManager.LoadScene((int)sceneVote.scene, LoadSceneMode.Single);
    }

    /// <summary>
    /// Transitions to the next level automatically.
    /// Next level is determined by incrementing the current scene index.
    /// </summary>
    public void TransitionToNextLevel()
    {
        Debug.Log("SceneTransitionHandler: TransitionToNextLevel() called!");

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        Debug.Log(
            $"SceneTransitionHandler: Current scene index: {currentSceneIndex}, Next scene index: {nextSceneIndex}, Total scenes in build: {SceneManager.sceneCountInBuildSettings}");

        // Check if next scene exists in build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load next scene by index
            Debug.Log($"SceneTransitionHandler: Loading scene index {nextSceneIndex}");
            SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
        }
        else
        {
            // No next level exists - go to fallback scene (e.g., main menu or end screen)
            Debug.LogWarning(
                $"SceneTransitionHandler: No next level found (index {nextSceneIndex} >= {SceneManager.sceneCountInBuildSettings}). Transitioning to fallback scene: {_fallbackScene}");
            SceneManager.LoadScene((int)_fallbackScene, LoadSceneMode.Single);
        }
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
