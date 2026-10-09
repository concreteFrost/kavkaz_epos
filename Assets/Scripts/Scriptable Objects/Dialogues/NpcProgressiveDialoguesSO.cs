using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NeutralDialogueLine
{
    [Tooltip("градация в соотношении с подсчётом выполнения глобальных квестов.")]
    [Range(0, 7)]
    public int minProgress;

    public List<DialogueLine> dialogueLines = new();


}


[CreateAssetMenu(fileName = "progress_dialogue_", menuName = ScriptablePaths.DIALOGUE_LINE__PATH + "/Progress Dialogue")]
public class NpcProgressiveDialoguesSO : ScriptableObject
{
    public List<DialogueLine> introductionDialogueLines = new List<DialogueLine>();
    [Tooltip("Нейтральные диалоговые линии по мере завершения игрового процесса. рекомендуемая градация 0,.29,.57,.86,1")]
    public List<NeutralDialogueLine> neutralDialogueLines = new List<NeutralDialogueLine>();


}
