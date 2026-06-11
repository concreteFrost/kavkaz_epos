using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFaderUI : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    private Coroutine fadeCoroutine;


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


    public void FadeIn(float duration)
    {
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
    }
}