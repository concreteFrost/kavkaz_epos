using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class QuestInfoUpdatedUI : MonoBehaviour
{
    [Header("Quest Panel Controls")]
    [SerializeField] TextMeshProUGUI text_questState;
    [SerializeField] TextMeshProUGUI text_questName;
    [SerializeField] GameObject questPanelWrapper;

    private float textShowDuration = 5f;
    private float textHideDelay = 2f;

    Queue<Action> textQueue = new Queue<Action>();
    private bool isQuestMessagesProcessing = false;
    private void Awake()
    {
        ToggleQuestPanel(false);
    }

    private void OnEnable()
    {
        DialogueController.QuestStarted += OnQuestStarted;
        DialogueController.QuestCompleted += OnQuestCompleted;
    }

    private void OnDisable()
    {
        DialogueController.QuestStarted -= OnQuestStarted;
        DialogueController.QuestCompleted -= OnQuestCompleted;
    }

    #region Quest Panel
    private void ToggleQuestPanel(bool isVisible) => questPanelWrapper.SetActive(isVisible);

    private void QuestCompleted(string questName)
    {
        text_questState.text = "QUEST COMPLETED!";
        text_questName.text = questName;
    }

    private void QuestStarted(string questName)
    {
        text_questState.text = "NEW QUEST STARTED";
        text_questName.text = questName;
    }

    private void OnQuestStarted(string questName)
    {
        textQueue.Enqueue(() => QuestStarted(questName));


        if (!isQuestMessagesProcessing)
        {
            StartCoroutine(ProcessQueue());
        }

    }

    private void OnQuestCompleted(string questName)
    {
        textQueue.Enqueue(() => QuestCompleted(questName));

        if (!isQuestMessagesProcessing)
        {
            StartCoroutine(ProcessQueue());
        }

    }

    private IEnumerator ProcessQueue()
    {
        isQuestMessagesProcessing = true;

        while (textQueue.Count > 0)
        {
            ToggleQuestPanel(true);
            var data = textQueue.Dequeue();
            data?.Invoke();

            yield return new WaitForSeconds(textShowDuration);
            ToggleQuestPanel(false);

            yield return new WaitForSeconds(textHideDelay);

        }

        ToggleQuestPanel(false);
        isQuestMessagesProcessing = false;

    }
    #endregion

    #region Test Functions
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        TestFunc();
    //    }
    //}

    //private void TestFunc()
    //{
    //    var rndName = UnityEngine.Random.Range(0, 100);
    //    var isCompleted = UnityEngine.Random.value > 0.5f;

    //    if (isCompleted)
    //    {
    //        OnQuestCompleted(rndName.ToString());
    //        return;
    //    }

    //    OnQuestStarted(rndName.ToString());


    //}

    #endregion
}
