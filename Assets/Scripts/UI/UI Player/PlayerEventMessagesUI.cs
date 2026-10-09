using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerEventMessagesUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject wrapper;
    [SerializeField] private TextMeshProUGUI textMessage;

    [Header("Settings")]
    [SerializeField] private float textShowDuration = 5f;
    [SerializeField] private float textHideDelay = 2f;

    private readonly Queue<string> messageQueue = new();

    private Coroutine messageCoroutine;

    public void Init()
    {
        HidePanel();
    }

    private void OnEnable()
    {
        Door.DoorMessage += OnDoorMessage;
        Bonfire.BonfireMessage += EnqueueMessage;

        QuestNpcDialogueController.QuestStarted += OnQuestStarted;
        QuestNpcDialogueController.QuestCompleted += OnQuestCompleted;
    }

    private void OnDisable()
    {
        Door.DoorMessage -= OnDoorMessage;
        Bonfire.BonfireMessage -= EnqueueMessage;

        QuestNpcDialogueController.QuestStarted -= OnQuestStarted;
        QuestNpcDialogueController.QuestCompleted -= OnQuestCompleted;
    }

    private void OnDoorMessage(string message)
    {
        EnqueueMessage(message);
    }

    private void OnQuestStarted(string questName)
    {
        EnqueueMessage($"Новое задание:\n{questName}");
    }

    private void OnQuestCompleted(string questName)
    {
        EnqueueMessage($"Выполнено:\n{questName}");
    }

    private void EnqueueMessage(string message)
    {
        messageQueue.Enqueue(message);

        if (messageCoroutine == null)
        {
            messageCoroutine = StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        while (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue();

            ShowMessage(message);

            yield return new WaitForSeconds(textShowDuration);

            HidePanel();

            if (messageQueue.Count > 0)
                yield return new WaitForSeconds(textHideDelay);
        }

        messageCoroutine = null;
    }

    private void ShowMessage(string message)
    {
        SetMessageText(message);
        wrapper.SetActive(true);
    }

    public void HidePanel()
    {
        SetMessageText(string.Empty);
        wrapper.SetActive(false);
    }

    private void SetMessageText(string message)
    {
        textMessage.text = message;
    }
}
