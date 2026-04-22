
using TMPro;
using UnityEngine;
using System.Collections;

public class BiomNameUI : MonoBehaviour
{
    #region Level Name
    [Header("Biom Name")]
    [SerializeField] GameObject levelNameWrapper;
    [SerializeField] TextMeshProUGUI levelName;

    private Coroutine levelNameRoutine;
    #endregion


    private void OnEnable()
    {

        LevelManager.LevelLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {

        LevelManager.LevelLoaded -= OnLevelLoaded;
    }

    private void ShowBiomNamePanel(bool show) => levelNameWrapper.SetActive(show);

    private void OnLevelLoaded(string levelName)
    {
        ShowBiomName(levelName);
    }

    public void ShowBiomName(string levelNameText)
    {
        if (levelNameRoutine != null)
        {
            StopCoroutine(levelNameRoutine);
        }

        levelNameRoutine = StartCoroutine(LevelNameRoutine(levelNameText));
    }

    private IEnumerator LevelNameRoutine(string levelNameText)
    {
        yield return new WaitForSeconds(1f); //небольшая задержка перед показом названия 

        ShowBiomNamePanel(true);
        levelName.text = levelNameText;

        CanvasGroup canvasGroup = levelNameWrapper.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = levelNameWrapper.AddComponent<CanvasGroup>();
        }

        float duration = 1.5f;
        float timer = 0;

        // Fade In
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / duration;
            yield return null;
        }

        canvasGroup.alpha = 1;

        // Hold
        yield return new WaitForSeconds(2f);

        // Fade Out
        timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = 1 - (timer / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        ShowBiomNamePanel(false);

        levelNameRoutine = null;
    }


}
