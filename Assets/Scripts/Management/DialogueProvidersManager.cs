using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueProvidersManager : MonoBehaviour
{
    public List<DialogueController> dialogueProviders = new List<DialogueController>();

    public void Init()
    {
        dialogueProviders = FindObjectsByType<DialogueController>(FindObjectsSortMode.None).ToList();

        foreach (var baseQuestProvider in dialogueProviders)
        {
            baseQuestProvider.Init();
        }
    }

    public List<NpcQuestsState> SaveQuestProvidersState()
    {
        List<NpcQuestsState> savedStates = new List<NpcQuestsState>();  

        foreach (var provider in dialogueProviders)
        {
            NpcQuestsState newState = new NpcQuestsState();
            newState.questProviderId = provider.providerId;

            foreach (var dialogues in provider.dialogueStates)
            {
                var questId = dialogues.questDialogue.questToGiveSO.id;

                var dialogueState = new DialogueState()
                {
                    questId = questId,
                    wasQuestCompleted = dialogues.wasQuestCompleted,
                    wasQuestStarted = dialogues.wasQuestStarted,
                    wasRewardGiven = dialogues.wasRewardGiven,
                };

                newState.dialogueStates.Add(dialogueState);
            }

            savedStates.Add(newState);    
        }
        return savedStates;
    }

    public void LoadQuestsState(LevelState data)
    {
        var questsData = data.questProviders;

        foreach (var quest in questsData)
        {
            var match = dialogueProviders.Find((x) => x.providerId == quest.questProviderId);

            if(match != null)
            {
                match.LoadData(quest);
            }
        }
    }
}




