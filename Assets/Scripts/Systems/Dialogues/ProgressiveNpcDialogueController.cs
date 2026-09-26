
using UnityEngine;
using System.Linq;



public class ProgressiveNpcDialogueController : DialogueController, IInteractable
{
    public NpcProgressiveDialoguesSO dialoguesSO;

    private void GiveNeautralLines()
    {
        int progress =
            GlobalQuestManager.Instance?.GetGlobalCompletedQuestState() ?? 0;

        var block = dialoguesSO.neutralDialogueLines
            .Where(x => progress >= x.minProgress)
            .OrderByDescending(x => x.minProgress)
            .FirstOrDefault();

        if (block != null)
        {
            FillQueue(block.dialogueLines);
        }
    }

    public override void StartDialogue()
    {
        if (dialoguesSO == null)
        {
            EndDialogue();
            return;
        }


        if (!wasIntroduced)
        {
            FillQueue(dialoguesSO.introductionDialogueLines);
            wasIntroduced = true;
            return;
        }

        GiveNeautralLines();

    }

    protected override void ShowNextLine()
    {
        if (!isDialogueActive)
            return;

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = dialogueQueue.Dequeue().dialogueLine;

        DialogueProceed?.Invoke(line);

        animatorController.PlayTalk();
    }

}
