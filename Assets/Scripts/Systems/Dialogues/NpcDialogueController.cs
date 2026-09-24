
using System.Linq;

public class NpcDialogueController : DialogueController, IInteractable
{
    public string InteractionName() => statsController.statsSO.characterName;
    public string ActionText() => "Говорить";

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

    protected override void GiveNeautralLines()
    {
        float progress =
            GlobalQuestManager.Instance?.GetGlobalCompletedQuestState() ?? 0f;

        var block = dialoguesSO.neutralDialogueLines
            .Where(x => progress >= x.minProgress)
            .OrderByDescending(x => x.minProgress)
            .FirstOrDefault();

        if (block != null)
        {
            FillQueue(block.dialogueLines);
        }
    }
}
