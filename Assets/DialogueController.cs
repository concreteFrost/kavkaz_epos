using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] NpcDialoguesSO dialoguesSO;

    [SerializeField] private QuestDialogue currentDialogue;

    NpcQuestProviderController questController;

    private Queue<DialogueLine> dialogueQueue = new();

    private void Awake()
    {
        questController = GetComponent<NpcQuestProviderController>();
    }



    public void StartDialogue()
    {
      
    }

    private void ShowNextLine()
    {
        if(dialogueQueue.Count == 0)
        {
            return;
        }

        var line = dialogueQueue.Dequeue();
        Debug.Log(line.dialogueLine);
    }

    private void FillDialogueQueue()
    {
       
    }

    private void EndDialogue()
    {
       
    }


}
