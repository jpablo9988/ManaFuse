using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    /// <summary>
    /// Very Placeholer. Loads One scene as single. Closes other screens.
    /// Doesn't support loading screens.
    /// </summary>
    /// <param name="scene">Scene to Go.</param>
    public void GoToScene(SceneTransitionVote sceneVote)
    {
        SceneManager.LoadScene((int)sceneVote.scene, LoadSceneMode.Single);
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
