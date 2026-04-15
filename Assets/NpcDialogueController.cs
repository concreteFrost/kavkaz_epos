using System;
using UnityEngine;

public class NpcDialogueController : DialogueController, IInteractable
{
    public Vector3 InitialPosition { get; set; }
    public Vector3 InitialRotation { get; set; }
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
