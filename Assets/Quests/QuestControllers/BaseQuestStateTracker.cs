using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Сериализуемое состояние провайдера квестов для сохранения/загрузки.
/// Хранит ID провайдера и список всех квестов с флагом получения награды.
/// </summary>
[System.Serializable]
public class QuestProviderState
{
    /// <summary>
    /// Уникальный идентификатор провайдера (NPC).
    /// </summary>
    public string providerId;

    /// <summary>
    /// Словарь всех квестов: ключ — ID квеста, значение — была ли выдана награда.
    /// </summary>
    public Dictionary<string, bool> allQuests = new Dictionary<string, bool>();
}



[Serializable]
public class ProvidedQuestState
{
    public string questId;
    public bool wasRewardGiven;

}

/// <summary>
/// Базовый контроллер NPC, который выдает квесты и награды.
/// Реализует интерфейс взаимодействия IInteractable.
/// </summary>
public abstract class BaseQuestStateTracker : MonoBehaviour
{
    /// <summary>
    /// Уникальный ID провайдера (генерируется через UniqueId).
    /// </summary>
    public string providerId;
    private UniqueId idGenerator;

    /// <summary>
    /// Текущий runtime-список квестов (изменяется во время игры).
    /// </summary>
    public List<ProvidedQuestState> quests = new List<ProvidedQuestState>();

  


    public virtual void Init()
    {
        idGenerator = GetComponent<UniqueId>();
        providerId = idGenerator.uniqueId;
        quests.Clear();

    }

    private ProvidedQuestState GetQuest(QuestSO questSO)=> quests.Find(q => q.questId == questSO.id);


    public void GiveNewQuest(QuestSO questSO)
    {
        var existing = GetQuest(questSO);

        //если квест уже есть в списке то ничего не делаем
        if (existing != null)
            return;

        var newQuest = new ProvidedQuestState
        {
            questId = questSO.id,
            wasRewardGiven = false
        };

        quests.Add(newQuest);

        if (!IsQuestStarted(questSO))
        {
            GlobalQuestManager.Instance.StartNewQuest(questSO);
        }
    }

    public bool IsQuestStarted(QuestSO questSO) => GlobalQuestManager.Instance.IsQuestStarted(questSO.id);


    public bool WasRewardGiven(QuestSO questSO)
    {
        var quest = quests.Find(q => q.questId == questSO.id);

        if (quest == null)
            return false;

        return quest.wasRewardGiven;
    }


    public void CompleteQuest(QuestSO questSO, List<ItemData> rewards)
    {
        var quest = GetQuest(questSO);

        if (quest == null)
        {
            var newQuest = new ProvidedQuestState
            {
                questId = questSO.id,
                wasRewardGiven = false
            };

            quests.Add(newQuest);

            return;
        }


        //GrandRewards?.Invoke(rewards);

        quest.wasRewardGiven = true;
    }



    /// <summary>
    /// Загружает состояние провайдера:
    /// - пересоздает runtime-квесты на основе сохраненных данных
    /// - восстанавливает флаг получения награды
    /// </summary>
    public void LoadState(QuestProviderState state)
    {
        //quests.Clear();

        //foreach (var savedQuest in state.allQuests)
        //{
        //    // Ищем соответствующий квест в исходном списке
        //    var initialQuestState = initialQuests.Find((x) => x.questSO.id == savedQuest.Key);

        //    if (initialQuestState != null)
        //    {
        //        Создаем копию состояния(важно, чтобы не мутировать initialQuests)
        //        var questInstance = new QuestRewardState
        //        {
        //            questSO = initialQuestState.questSO,
        //            wasRewardGranted = savedQuest.Value,
        //            rewards = initialQuestState.rewards
        //        };

        //        runtimeQuests.Add(questInstance);
        //    }
        //}
    }
}