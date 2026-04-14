using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestProvidersManager : MonoBehaviour
{
    public List<BaseQuestStateTracker> baseQuestProviders = new List<BaseQuestStateTracker>();

    public void Init()
    {
        baseQuestProviders = FindObjectsByType<BaseQuestStateTracker>(FindObjectsSortMode.None).ToList();

        foreach (var baseQuestProvider in baseQuestProviders)
        {
            baseQuestProvider.Init();
        }
    }

    public List<QuestProviderState> SaveQuestProvidersState()
    {
        List<QuestProviderState> savedStates = new List<QuestProviderState>();  

        foreach (var baseQuestProvider in baseQuestProviders)
        {
            Dictionary<string, bool> questsState= new Dictionary<string,bool>();

           
            baseQuestProvider.quests.ForEach(quest => questsState[quest.questId] = quest.wasRewardGiven);
           
            QuestProviderState stateToSave = new QuestProviderState
            {
                allQuests = questsState,
                providerId = baseQuestProvider.providerId,
            };

            savedStates.Add(stateToSave);
        }
        return savedStates;
    }

    public void LoadQuestsState(LevelState data)
    {
        var questsData = data.questProviders;

        foreach (var quest in questsData)
        {
            var match = baseQuestProviders.Find((x) => x.providerId == quest.providerId);

            if(match != null)
            {
                match.LoadState(quest);
            }
        }
    }
}




