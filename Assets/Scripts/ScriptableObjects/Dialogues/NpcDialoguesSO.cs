using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class DialogueLine
{
    [TextArea]
    public string dialogueLine;
 
}

[Serializable]
public class NeutralDialogueLine
{
    [Tooltip("градация в соотношении с подсчётом выполнения глобальных квестов.")]
    [Range(0, 1)]
    public float minProgress;

    public List<DialogueLine> dialogueLines = new();

    
}

[Serializable]
public class NpcQuestDialogue
{
    public QuestSO questToGiveSO;

    public List<DialogueLine> questStartedLines = new List<DialogueLine>();
    public List<DialogueLine> questCompletedLines = new List<DialogueLine>();
    public List<DialogueLine> questInProgressLines = new List<DialogueLine>();  

    public List<ItemData> rewards = new List<ItemData>();   

}

[CreateAssetMenu(fileName = "DialogueLine_", menuName = ScriptablePaths.DIALOGUE_LINE__PATH + "/Npc Dialogue")]
public class NpcDialoguesSO : ScriptableObject
{

    public List<NpcQuestDialogue> questDialogueLines = new List<NpcQuestDialogue>();
    public List<DialogueLine> introductionDialogueLines = new List<DialogueLine>();
    [Tooltip("Нейтральные диалоговые линии по мере завершения игрового процесса. рекомендуемая градация 0,.29,.57,.86,1")]
    public List<NeutralDialogueLine> neutralDialogueLines = new List<NeutralDialogueLine>();  


}
