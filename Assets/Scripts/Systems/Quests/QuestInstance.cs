using NUnit.Framework;
using System;
using UnityEngine;

[Serializable]
public class QuestState
{
    public string questId;
    public bool isStarted;
    public bool isCompleted;
    public bool wasRewardGiven;
}

[Serializable]
public class QuestInstance
{
    public QuestState state;

    [HideInInspector]
    public QuestSO definition;

    public void Start(QuestSO questSO)
    {
        definition = questSO;

        state = new QuestState
        {
            questId = questSO.id,
            isStarted = true,
            isCompleted = false,
            wasRewardGiven = false
        };
    }

    public void Complete()
    {
        state.isCompleted = true;
    }

    public void LoadQuest(QuestSO questSO, QuestState loadedState)
    {
        definition = questSO;

        state = new QuestState
        {
            questId = questSO.id,
            isCompleted = loadedState.isCompleted,
            wasRewardGiven = loadedState.wasRewardGiven,
            isStarted = loadedState.isStarted
        };

        
    }

}


