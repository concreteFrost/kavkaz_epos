using System.Collections.Generic;
using UnityEngine;

public abstract class QuestCompletionListener : MonoBehaviour
{

    private void OnEnable()
    {
        QuestInstance.QuestCompleted += OnQuestCompleted;
    }

    private void OnDisable()
    {
        QuestInstance.QuestCompleted -= OnQuestCompleted;
    }

    private void OnQuestCompleted(QuestSO questSO)
    {

        React(questSO);
    }

    protected abstract void React(QuestSO questSO);
}

