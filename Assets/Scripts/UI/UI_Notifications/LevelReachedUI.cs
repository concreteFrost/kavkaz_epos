
using TMPro;
using UnityEngine;
using System.Collections;

public class LevelReachedUI : MonoBehaviour
{

    [Header("New Level Achieved Controls")]
    [SerializeField] TextMeshProUGUI text_NewLevelAchieved;
    Coroutine levelUpdatedCoroutine = null;

    private void OnEnable()
    {
        CharacterLevelController.NewLevelReachedWithMessage += OnNeveLevelReached;
    }

    private void OnDisable()
    {

        CharacterLevelController.NewLevelReachedWithMessage -= OnNeveLevelReached;
    }

    private void ToggleLevelAchievedText(bool isVisible) => text_NewLevelAchieved.gameObject.SetActive(isVisible);

    private void ShowLevelUpdated()
    {
        if (levelUpdatedCoroutine != null)
        {
            StopCoroutine(levelUpdatedCoroutine);
            levelUpdatedCoroutine = null;
        }

        levelUpdatedCoroutine = StartCoroutine(ShowLevelUpdatedCoroutine());
    }

    private void OnNeveLevelReached()
    {
        Debug.Log("new level");
        ShowLevelUpdated();
    }

    IEnumerator ShowLevelUpdatedCoroutine()
    {
        ToggleLevelAchievedText(true);
        yield return new WaitForSeconds(3);
        ToggleLevelAchievedText(false);

        levelUpdatedCoroutine = null;
    }

}
