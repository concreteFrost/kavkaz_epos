using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFaderUI : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private RectTransform loadingSpinner;
    [SerializeField] private float spinnerSpeed = 180f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    private void OnEnable()
    {
        SceneTransitionManager.TransitionStarted += FadeIn;
        SceneTransitionManager.TransitionFinished += FadeOut;
    }

    private void OnDisable()
    {
        SceneTransitionManager.TransitionStarted -= FadeIn;
        SceneTransitionManager.TransitionFinished -= FadeOut;
    }

    private void Update()
    {
        if (loadingScreen != null &&
            loadingScreen.activeSelf &&
            loadingSpinner != null)
        {
            loadingSpinner.Rotate(0f, 0f, -spinnerSpeed * Time.deltaTime);
        }
    }

    public void FadeIn(float duration)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        StartFade(0f, 1f, duration);
    }

    public void FadeOut(float duration)
    {
        StartFade(1f, 0f, duration);
    }

    private void StartFade(float from, float to, float duration)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine(from, to, duration));
    }

    private IEnumerator FadeRoutine(float from, float to, float duration)
    {
        float elapsed = 0f;

        Color color = fadeImage.color;
        color.a = from;
        fadeImage.color = color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(from, to, elapsed / duration);
            fadeImage.color = color;

            yield return null;
        }

        color.a = to;
        fadeImage.color = color;

        if (Mathf.Approximately(to, 0f) && loadingScreen != null)
            loadingScreen.SetActive(false);

        fadeCoroutine = null;
    }
}