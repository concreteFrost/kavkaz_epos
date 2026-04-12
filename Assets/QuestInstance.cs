using System;
using UnityEngine;

[Serializable]
public class QuestState
{
    public string questId;
    public bool isCompleted;
    public bool rewardTaken;
}

[Serializable]
public class QuestInstance
{
    public QuestState state;
    [HideInInspector] public QuestSO definition;
    public static Action<QuestSO> QuestCompleted;

    public void Init(QuestSO questSO)
    {
        definition = questSO;

        state = new QuestState
        {
            questId = questSO.id,
            isCompleted = false
        };
    }

    public void LoadQuest(QuestSO questSO, bool isCompleted)
    {
        definition = questSO;

        state = new QuestState
        {
            questId = questSO.id,
            isCompleted = isCompleted
        };
    }

    public void Complete()
    {
        QuestCompleted?.Invoke(definition);

        if (state.isCompleted) return;

        Debug.Log($"{definition.questName} completed");

        state.isCompleted = true;

        definition.GetRewards();

        
    }
}
