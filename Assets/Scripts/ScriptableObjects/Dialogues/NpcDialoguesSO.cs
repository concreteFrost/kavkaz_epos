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
    public List<DialogueLine> neutralDialogueLines = new List<DialogueLine>();  


}
