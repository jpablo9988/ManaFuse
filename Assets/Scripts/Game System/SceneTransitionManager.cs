using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Optional Transition Effect")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(SceneIndex sceneIndex)
    {
        StartCoroutine(Transition((int)sceneIndex));
    }

    private IEnumerator Transition(int sceneIndex)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (fadeCanvasGroup != null)
            yield return StartCoroutine(FadeOut());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
            yield return null;

        if (fadeCanvasGroup != null)
            yield return StartCoroutine(FadeIn());

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null && fadeCanvasGroup != null)
            fadeCanvasGroup.transform.SetParent(canvas.transform, false);
    }

    private IEnumerator FadeOut()
    {
        fadeCanvasGroup.blocksRaycasts = true;
        fadeCanvasGroup.interactable = true;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;
        fadeCanvasGroup.interactable = false;
    }
}
