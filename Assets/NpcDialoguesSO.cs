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
public class QuestDialogue
{
    public QuestSO questToGiveSO;
   
    public List<DialogueLine> questStartedLines = new List<DialogueLine>();
    public List<DialogueLine> questCompletedLines = new List<DialogueLine>();
    public List<DialogueLine> questInProgressLines = new List<DialogueLine>();  

    public bool willTriggerQuest = false;
}

[CreateAssetMenu(fileName = "DialogueLine_", menuName = ScriptablePaths.DIALOGUE_LINE__PATH + "/Npc Dialogue")]
public class NpcDialoguesSO : ScriptableObject
{
    public string npcId;
   
    public List<QuestDialogue> questDialogueLines = new List<QuestDialogue>();  
    public List<DialogueLine> neutralDialogueLines = new List<DialogueLine>();  




}
