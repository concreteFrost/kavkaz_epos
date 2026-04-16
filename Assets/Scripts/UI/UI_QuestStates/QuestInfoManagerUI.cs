using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;

public class QuestInfoManagerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_questState;
    [SerializeField] TextMeshProUGUI text_questName;
    [SerializeField] GameObject wrapper;

    private float textShowDuration = 5f;
    private float textHideDelay = 2f;

    Queue<Action> textQueue = new Queue<Action>();
    private bool isProcessing = false;

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

    private void Awake()
    {
        HidePanel();
    }

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

    private void ShowPanel() => wrapper.SetActive(true);
    private void HidePanel()=> wrapper.SetActive(false);

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


        if (!isProcessing)
        {
            StartCoroutine(ProcessQueue());
        }

    }

    private void OnQuestCompleted(string questName)
    {
        textQueue.Enqueue(() => QuestCompleted(questName));

        if (!isProcessing)
        {
            StartCoroutine(ProcessQueue());
        }

    }

    private IEnumerator ProcessQueue()
    {
        isProcessing = true;

        while (textQueue.Count > 0)
        {
            ShowPanel();
            var data = textQueue.Dequeue();
            data?.Invoke();

            yield return new WaitForSeconds(textShowDuration);
            HidePanel();

            yield return new WaitForSeconds(textHideDelay);

        }

        HidePanel(); 
        isProcessing = false;
        

    }


}
