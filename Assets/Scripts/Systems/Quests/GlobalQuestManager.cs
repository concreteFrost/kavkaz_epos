using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Глобальный менеджер квестов:
/// - хранит все активные/завершенные квесты
/// - отвечает за создание, завершение и сохранение
/// </summary>
public class GlobalQuestManager : MonoBehaviour
{

    public static GlobalQuestManager Instance;

    /// <summary>
    /// Квесты, которые автоматически создаются при старте.
    /// </summary>
    [SerializeField] List<QuestSO> defaultQuests = new List<QuestSO>();

    /// <summary>
    /// Все квесты в игре (runtime).
    /// </summary>
    public List<QuestInstance> allQuests = new List<QuestInstance>();

    /// <summary>
    /// Выдать награды за квест
    /// </summary>
    public static Action<List<ItemData>> GrandRewards;
    public static Action<QuestSO> QuestCompleted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// Инициализация: создает дефолтные квесты (один раз).
    /// </summary>
    public void Init()
    {
        allQuests.Clear();

        foreach (var questSO in defaultQuests)
        {
            StartNewQuest(questSO);
        }
    }


    /// <summary>
    /// Возвращает прогрессию выполненых глобальных квестов в проц. соотношении
    /// </summary>
    /// <returns></returns>
    public int GetGlobalCompletedQuestState()
    {
        if (defaultQuests.Count == 0) return 0;

        int completedQuests = 0;

        foreach (var quest in defaultQuests)
        {
            var match = allQuests.Find(x => x.definition.id == quest.id);

            if (match != null && match.state.isCompleted)
            {
                completedQuests++;
            }
        }

        return completedQuests;
    }

    /// <summary>
    /// Создает новый квест на основе QuestSO.
    /// </summary>
    public QuestInstance StartNewQuest(QuestSO questSO)
    {
        if (IsQuestCompleted(questSO)) return null;

        QuestInstance newQuest = new QuestInstance();
        newQuest.Start(questSO);

        allQuests.Add(newQuest);

        return newQuest;
    }

    /// <summary>
    /// Завершает квест:
    /// - если его нет — сначала создает
    /// </summary>
    public void CompleteQuest(QuestSO questSO)
    {
        var targetQuest = allQuests.Find(x => x.definition.id == questSO.id);

        if (targetQuest == null)
        {
            targetQuest = StartNewQuest(questSO);
        }

        targetQuest.Complete();
        QuestCompleted?.Invoke(targetQuest.definition);
    }

    /// <summary>
    /// Повторно применяет состояние квестов (например, после загрузки).
    /// </summary>
    public void GetCurrentQuestsState()
    {
        //foreach (var quest in allQuests)
        //{
        //    if (quest.state.isCompleted)
        //    {
        //        quest.Complete();
        //    }
        //}
    }

    /// <summary>
    /// Проверяет, завершен ли квест.
    /// </summary>
    public bool IsQuestCompleted(QuestSO quest)
    {
        if (allQuests.Count == 0) return false;

        var targetQuest = allQuests.Find(x => x.state.questId == quest.id);

        if (targetQuest == null) return false;

      

        return targetQuest.state.isCompleted;
    }

    public bool WasRewardGiven(QuestSO quest)
    {
        var targetQuest = allQuests.Find(x => x.state.questId == quest.id);

        if (targetQuest == null) return false;

        return targetQuest.state.wasRewardGiven;
    }

    public void GiveReward(QuestSO quest)
    {
        var targetQuest = allQuests.Find(x => x.state.questId == quest.id);

        if (targetQuest == null || !targetQuest.state.isCompleted || targetQuest.state.wasRewardGiven) return;
        if (!targetQuest.definition.TryCreateRewards(out var items)) return;
        if (items.Count > 0 && GrandRewards == null) return;

        // Guard against reentrant callbacks granting the same reward twice.
        targetQuest.state.wasRewardGiven = true;
        if (items.Count > 0)
            GrandRewards.Invoke(items);
    }

    /// <summary>
    /// Собирает данные квестов для сохранения.
    /// </summary>
    public List<QuestState> SaveQuestsState()
    {
        List<QuestState> questsToSave = new List<QuestState>();

        foreach (var quest in allQuests)
        {
            QuestState questState = new QuestState()
            {
                questId = quest.state.questId,
                isCompleted = quest.state.isCompleted,
                wasRewardGiven = quest.state.wasRewardGiven,
                isStarted = quest.state.isStarted
            };

            questsToSave.Add(questState);
        }

        return questsToSave;
    }

    /// <summary>
    /// Загружает квесты из сохранения.
    /// </summary>
    public void LoadQuestsData(SaveGameData data)
    {
        allQuests.Clear();

        var questsData = data.questsStates;

        if (questsData.Count == 0) return;

        var resources = Resources.LoadAll<QuestSO>("Systems/Quests/");

        foreach (var load in questsData)
        {
            foreach (var resourceQuest in resources)
            {
                if (load.questId == resourceQuest.id)
                {
                    QuestInstance loadedQuest = new QuestInstance();
                    loadedQuest.LoadQuest(resourceQuest, load);

                    allQuests.Add(loadedQuest);
                }
            }
        }
    }

   
}