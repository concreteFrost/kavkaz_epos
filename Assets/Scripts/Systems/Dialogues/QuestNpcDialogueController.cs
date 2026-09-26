using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestNpcDialogueController : DialogueController
{
    [SerializeField] protected NpcQuestDialoguesSO dialoguesSO;
    public QuestSO questToGiveSO;

    //ДЛЯ QuestInfoManagerUI
    public static Action<string> QuestStarted;
    public static Action<string> QuestCompleted;

    protected bool willSayThankYou = false;

    public bool IsCompletedGlobal()
    {
        if (GlobalQuestManager.Instance == null) return false;

        return GlobalQuestManager.Instance.IsQuestCompleted(questToGiveSO);
    }

    public bool WasRewardGiven()
    {
        if (GlobalQuestManager.Instance == null) return false;

        return GlobalQuestManager.Instance.WasRewardGiven(questToGiveSO);
    }

    public void GiveRewards()
    {
        if (GlobalQuestManager.Instance == null) return;

        GlobalQuestManager.Instance.GiveReward(questToGiveSO);
    }




    // 🔹 Входная точка
    public override void StartDialogue()
    {
        if (dialoguesSO == null)
        {
            EndDialogue();
            return;
        }
            

        if (!wasIntroduced)
        {
            FillQueue(dialoguesSO.questStartedLines);
            GiveNewQuest();
            wasIntroduced = true;

            return;
        }

        if (!IsCompletedGlobal())
        {
            FillQueue(dialoguesSO.questInProgressLines);
           
            return;
        }

        if (!WasRewardGiven())
        {
            willSayThankYou = true;

            FillQueue(dialoguesSO.questCompletedLines);
            GiveRewards();

            var questName = questToGiveSO.questName;
            QuestCompleted?.Invoke(questName);
    
            return;
        }


        FillQueue(dialoguesSO.neutralLines);
        
    }

    protected override void ShowNextLine()
    {

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        var line = dialogueQueue.Dequeue().dialogueLine;

        DialogueProceed?.Invoke(line);

        if (willSayThankYou)
        {
            animatorController.PlayThankYou();
            willSayThankYou = false;
            return;
        }

        animatorController.PlayTalk();
    }

    private void GiveNewQuest()
    {
        if (IsCompletedGlobal()) return;

        GlobalQuestManager.Instance.StartNewQuest(questToGiveSO);

        var questName = questToGiveSO.questName;
        QuestStarted?.Invoke(questName);
    }

   


}
