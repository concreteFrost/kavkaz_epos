using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ѕазовый класс дл€ объектов, которые подписываютс€ на глобальное событие завершени€ квеста
/// и выполн€ют реакцию независимо от взаимодействи€ с игроком.
/// </summary>
public abstract class QuestCompletionObserver : MonoBehaviour
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

