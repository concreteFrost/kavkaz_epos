using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestProvidersManager : MonoBehaviour
{
    public List<BaseQuestProviderController> baseQuestProviders = new List<BaseQuestProviderController>();

    public void Init()
    {
        baseQuestProviders = FindObjectsByType<BaseQuestProviderController>(FindObjectsSortMode.None).ToList();

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

            baseQuestProvider.runtimeQuests.ForEach(quest => questsState[quest.questSO.id] = quest.wasRewardGranted);
           
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




