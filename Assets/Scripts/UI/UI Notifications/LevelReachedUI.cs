
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelReachedUI : MonoBehaviour
{

    [Header("Уведомление о новом уровне")]
    [Tooltip("Маска раскрытия панели слева направо.")]
    [SerializeField] private RectMask2D revealMask;
    [Tooltip("Длительность раскрытия панели в секундах.")]
    [SerializeField] private float revealDuration;
    [Tooltip("Длительность показа после раскрытия в секундах.")]
    [SerializeField] private float displayDuration;
    Coroutine levelUpdatedCoroutine = null;

    public void Init()
    {
        ToggleLevelAchievedText(false);
    }

    private void ToggleLevelAchievedText(bool isVisible) => revealMask.gameObject.SetActive(isVisible);

    private void ShowLevelUpdated()
    {
        if (levelUpdatedCoroutine != null)
        {
            StopCoroutine(levelUpdatedCoroutine);
            levelUpdatedCoroutine = null;
        }

        levelUpdatedCoroutine = StartCoroutine(ShowLevelUpdatedCoroutine());
    }

    public void OnNeveLevelReached()
    {
        
        ShowLevelUpdated();
    }

    IEnumerator ShowLevelUpdatedCoroutine()
    {
        float width = revealMask.rectTransform.rect.width;
        revealMask.padding = new Vector4(0f, 0f, width, 0f);
        ToggleLevelAchievedText(true);

        float elapsed = 0f;
        while (elapsed < revealDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, elapsed / revealDuration);
            revealMask.padding = new Vector4(0f, 0f, Mathf.Lerp(width, 0f, progress), 0f);
            yield return null;
        }

        revealMask.padding = Vector4.zero;
        yield return new WaitForSeconds(displayDuration);
        ToggleLevelAchievedText(false);

        levelUpdatedCoroutine = null;
    }

}
