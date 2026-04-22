using System;
using UnityEngine;

public class NpcDialogueController : DialogueController, IInteractable
{
    public string InteractionName() => statsController.statsSO.characterName;
    public string ActionText() => "Talk";

    public bool HasInteracted { get; set; } 

    public bool CanInteract() => true;
    public void Interact(IInteractor picker)
    {
        if (!isDialogueActive)
        {
            StartDialogue();
            isDialogueActive = true;
            GameStateManager.GameStateChanged?.Invoke(GameState.Dialogue);
            ShowNextLine();
        }
        else
        {
            ShowNextLine();
        }
    }

    public ItemInteractionType InteractType() => ItemInteractionType.NPC;
}
