using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueController : MonoBehaviour, IInteractable
{
    protected BaseHumanoidAnimatorController animatorController;
    protected CharacterStatsController statsController;

    protected Queue<DialogueLine> dialogueQueue = new();

    public bool isDialogueActive = false;

    public static Action<string> DialogueStarted;
    public static Action<string> DialogueProceed;
    public static Action DialogueCompleted;

    protected bool dialogueCompleted;

    public bool wasIntroduced;


    public string InteractionName() =>
        statsController.statsSO.characterName;

    public string ActionText() => "Говорить";

    public bool HasInteracted { get; set; }

    public bool CanInteract() => true;

    public ItemInteractionType InteractType() =>
        ItemInteractionType.NPC;

    public virtual void Init(
        BaseHumanoidAnimatorController animatorController,
        CharacterStatsController statsController)
    {
        this.animatorController = animatorController;
        this.statsController = statsController;
    }


    private void OnEnable()
    {
        GameStateManager.GameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.GameStateChanged -= OnGameStateChanged;
        // На случай уничтожения/отключения объекта
        UnsubscribeDialogueInput();
    }

    private void SubscribeDialogueInput()
    {
        PlayerGameInput.ProceedDialogue += ShowNextLine;
        PlayerGameInput.QuitDialogue += ClearDialogue;
    }

    private void UnsubscribeDialogueInput()
    {
        PlayerGameInput.ProceedDialogue -= ShowNextLine;
        PlayerGameInput.QuitDialogue -= ClearDialogue;
    }


    public void Interact(IInteractor picker)
    {
        // Первый вход в диалог
        if (!isDialogueActive)
        {
            SubscribeDialogueInput();

            isDialogueActive = true;
            dialogueCompleted = false;

            GameStateManager.GameStateChanged?.Invoke(
                GameState.Dialogue);

            DialogueStarted?.Invoke(statsController.statsSO.characterName);
            StartDialogue();
           
            // Показываем первую реплику
            ShowNextLine();
            return;
        }

        ShowNextLine();
    }

    public abstract void StartDialogue();


    protected abstract void ShowNextLine();


    protected void FillQueue(IEnumerable<DialogueLine> lines)
    {
        dialogueQueue.Clear();

        foreach (var line in lines)
        {
            dialogueQueue.Enqueue(line);
        }
    }


    protected virtual void EndDialogue()
    {

        ClearDialogue();

        GameStateManager.GameStateChanged?.Invoke(
            GameState.Game);

    }

    protected void ClearDialogue()
    {
        UnsubscribeDialogueInput();

        isDialogueActive = false;
        dialogueCompleted = true;

        DialogueCompleted?.Invoke();

        animatorController.ResetAnimator();
    }


    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Dialogue)
            return;
        
        ClearDialogue();
    }




    public void LoadData(bool wasIntroduced)
    {
        this.wasIntroduced = wasIntroduced;
    }
}