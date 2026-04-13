using System;
using System.Collections.Generic;

[Serializable]
public class QuestRewardState
{
    public QuestSO questSO;
    public List<ItemData> rewards = new List<ItemData>();
    public bool wasRewardGranted = false;
}

public class QuestProvider
{
    public static Action<List<ItemData>> GrandRewards;

    public QuestRewardState currentQuest;

    public void SetCurrentQuest(QuestRewardState newQuest)
    {
        currentQuest = newQuest;

        var isQuestAlreadyExists = GlobalQuestManager.Instance.allQuests.Find((x) => x.state.questId == newQuest.questSO.id);

        if (isQuestAlreadyExists == null)
        {
            GlobalQuestManager.Instance.StartNewQuest(newQuest.questSO);
        }
    }

    public bool IsCurrentQuestCompleted()
    {
        if (currentQuest == null) return false;

        return GlobalQuestManager.Instance
            .IsQuestCompleted(currentQuest.questSO);
    }

    public bool TryGiveReward(QuestRewardState state)
    {
        if (state == null) return false;
        if (state.wasRewardGranted) return false;

        if (!GlobalQuestManager.Instance.IsQuestCompleted(state.questSO))
            return false;

        GiveReward(state);
        state.wasRewardGranted = true;

        return true;
    }

    private void GiveReward(QuestRewardState state)
    {
        GrandRewards?.Invoke(state.rewards);
    }
}
