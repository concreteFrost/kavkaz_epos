using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class DialogueLine
{
    [TextArea]
    public string dialogueLine;
 
}


[CreateAssetMenu(fileName = "quest_dialogue_", menuName = ScriptablePaths.DIALOGUE_LINE__PATH + "/Quest Dialogue")]
public class NpcQuestDialoguesSO : ScriptableObject
{

    public QuestSO questToGiveSO;

    public List<DialogueLine> questStartedLines = new List<DialogueLine>();
    public List<DialogueLine> questCompletedLines = new List<DialogueLine>();
    public List<DialogueLine> questInProgressLines = new List<DialogueLine>();

    public List<DialogueLine> neutralLines = new List<DialogueLine>();

    public List<ItemData> rewards = new List<ItemData>();

}
