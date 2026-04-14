using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DialogueQuestState
{
    public NpcQuestDialogue questDialogue;

    public bool wasQuestCompleted;
    public bool wasRewardGiven;
    public bool wasQuestStarted;

    public DialogueQuestState(NpcQuestDialogue questDialogue)
    {
        this.questDialogue = questDialogue;
        wasQuestCompleted = false;
        wasRewardGiven = false;
        wasQuestStarted = false;
    }

    public QuestSO Quest => questDialogue.questToGiveSO;

    public bool IsCompletedGlobal()
    {
        return GlobalQuestManager.Instance.IsQuestCompleted(questDialogue.questToGiveSO);
    }
}

public class DialogueController : MonoBehaviour
{
    [SerializeField] private NpcDialoguesSO dialoguesSO;

    private Queue<DialogueLine> dialogueQueue = new();
    public List<DialogueQuestState> npcQuestStates = new();

    public bool isDialogueActive = false;

    public static Action<List<ItemData>> GrandRewards;

    private void Awake()
    {
        foreach (var dialogue in dialoguesSO.questDialogueLines)
        {
            npcQuestStates.Add(new DialogueQuestState(dialogue));
        }
    }

    // 🔹 Входная точка
    public void StartDialogue()
    {
        if (dialoguesSO == null)
            return;

        // 1. завершённый квест без награды
        if (TryHandleCompletedQuest())
            return;

        // 2. текущий квест
        if (TryHandleCurrentQuest())
            return;

        // 3. fallback
        FillQueue(dialoguesSO.neutralDialogueLines);
    }

    // 🔹 COMPLETED
    private bool TryHandleCompletedQuest()
    {
        var state = npcQuestStates.FirstOrDefault(d =>
            d.IsCompletedGlobal() && !d.wasRewardGiven);

        if (state == null)
            return false;

        FillQueue(state.questDialogue.questCompletedLines);

        state.wasQuestCompleted = true;
        state.wasRewardGiven = true;

        if (!state.wasQuestStarted)
        {
            state.wasQuestStarted = true;
        }

        GrandRewards?.Invoke(state.questDialogue.rewards);

        return true;
    }

    // 🔹 CURRENT
    private bool TryHandleCurrentQuest()
    {
        var state = npcQuestStates.FirstOrDefault(d => !d.wasQuestCompleted);

        if (state == null)
            return false;

        if (state.wasQuestStarted)
        {
            FillQueue(state.questDialogue.questInProgressLines);
            return true;
        }

        FillQueue(state.questDialogue.questStartedLines);
        StartCoroutine(StartQuestAfterDialogue(state));

        return true;
    }

    // 🔹 Coroutine
    private IEnumerator StartQuestAfterDialogue(DialogueQuestState state)
    {
        yield return new WaitUntil(() => !isDialogueActive);

        GlobalQuestManager.Instance.StartNewQuest(state.Quest);
        state.wasQuestStarted = true;
    }

    // 🔹 Dialogue flow
    public void Interact()
    {
        if (!isDialogueActive)
        {
            StartDialogue();
            isDialogueActive = true;
        }

        ShowNextLine();
    }

    protected void ShowNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = dialogueQueue.Dequeue();
        Debug.Log(line.dialogueLine);
    }

    private void FillQueue(List<DialogueLine> lines)
    {
        dialogueQueue.Clear();

        foreach (var line in lines)
        {
            dialogueQueue.Enqueue(line);
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        Debug.Log("Dialogue ended");
    }
}