using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class NpcQuestState
{
    public NpcQuestDialogue questDialogue;

    public bool wasQuestCompleted;
    public bool wasRewardGiven;
    public bool wasQuestStarted;

    public NpcQuestState(NpcQuestDialogue questDialogue)
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

[System.Serializable]
public class DialogueState
{
    public string questId;

    public bool wasQuestCompleted;
    public bool wasRewardGiven;
    public bool wasQuestStarted;
}

[System.Serializable]
public class NpcQuestsState
{
    public string questProviderId;
    public List<DialogueState> dialogueStates = new List<DialogueState>();
}

public class DialogueController : MonoBehaviour
{
    [SerializeField] private NpcDialoguesSO dialoguesSO;

    public string providerId;

    private Queue<DialogueLine> dialogueQueue = new();
    public List<NpcQuestState> dialogueStates = new();

    public bool isDialogueActive = false;

    public static Action<List<ItemData>> GrandRewards;

    public static Action<string> DialogueStarted;
    public static Action<string> DialogueProceed;
    public static Action DialogueCompleted;

    private bool dialogueCompleted;

    private void OnEnable()
    {
        GameStateManager.GameStateChanged += OnGameStateChanged;
        PlayerGameInput.ProceedDialogue += ShowNextLine;
        PlayerGameInput.QuitDialogue += OnDialogueQuit;
    }

    
    private void OnDisable()
    {
        GameStateManager.GameStateChanged -= OnGameStateChanged;
        PlayerGameInput.ProceedDialogue -= ShowNextLine;
        PlayerGameInput.QuitDialogue -= OnDialogueQuit;

    }

    public void Init()
    {
        providerId = GetComponent<UniqueId>().uniqueId;

        foreach (var dialogue in dialoguesSO.questDialogueLines)
        {
            dialogueStates.Add(new NpcQuestState(dialogue));
        }
    }

    public void LoadData(NpcQuestsState state)
    {
        foreach (var s in state.dialogueStates)
        {
            var match = dialogueStates.Find((x) => x.questDialogue.questToGiveSO.id == s.questId);

            if(match != null)
            {
                match.wasQuestCompleted = s.wasQuestCompleted;
                match.wasQuestStarted = s.wasQuestStarted;  
                match.wasRewardGiven = s.wasRewardGiven;    
            }
        }
    }

    // 🔹 Входная точка
    public void StartDialogue()
    {
        if (dialoguesSO == null)
            return;

        DialogueStarted?.Invoke("Npc Name");

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
        var state = dialogueStates.FirstOrDefault(d =>
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
        var state = dialogueStates.FirstOrDefault(d => !d.wasQuestCompleted);

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
    private IEnumerator StartQuestAfterDialogue(NpcQuestState state)
    {
        yield return new WaitUntil(() => dialogueCompleted);

        GlobalQuestManager.Instance.StartNewQuest(state.Quest);
        state.wasQuestStarted = true;

        dialogueCompleted = false; // сброс
    }


    protected void ShowNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = dialogueQueue.Dequeue();
        DialogueProceed?.Invoke(line.dialogueLine);
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
        GameStateManager.GameStateChanged?.Invoke(GameState.Game);

        isDialogueActive = false;
        dialogueCompleted = true;

        DialogueCompleted?.Invoke();

    }

    #region Events Handler
    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Dialogue) return;

        OnDialogueQuit();
    }

    private void OnDialogueQuit()
    {
        dialogueQueue.Clear();
        isDialogueActive = false;
        dialogueCompleted = false; // 🔥 важно

        DialogueCompleted?.Invoke();
    }
    #endregion
}