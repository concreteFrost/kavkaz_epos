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

/// <summary>
/// Базовый контроллер NPC, который выдает квесты и награды.
/// Реализует интерфейс взаимодействия IInteractable.
/// </summary>
public abstract class BaseQuestProviderController : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Уникальный ID провайдера (генерируется через UniqueId).
    /// </summary>
    public string providerId;

    private UniqueId idGenerator;

    /// <summary>
    /// Логика работы квестов (назначение, выдача наград).
    /// </summary>
    private QuestProvider provider;

    /// <summary>
    /// Исходный список квестов (настраивается в инспекторе).
    /// </summary>
    public List<QuestRewardState> initialQuests = new List<QuestRewardState>();

    /// <summary>
    /// Текущий runtime-список квестов (изменяется во время игры).
    /// </summary>
    public List<QuestRewardState> runtimeQuests = new List<QuestRewardState>();

    #region Interactable Contract


    public ItemInteractionType InteractType() => ItemInteractionType.NPC;

    public Vector3 InitialPosition { get; set; }

    public Vector3 InitialRotation { get; set; }

    public bool HasInteracted { get; set; }

    public bool CanInteract() => true;

    #endregion


    public virtual void Init()
    {
        idGenerator = GetComponent<UniqueId>();

        providerId = idGenerator.uniqueId;

        runtimeQuests.Clear();
        runtimeQuests.AddRange(initialQuests);

        provider = new QuestProvider();
    }

    /// <summary>
    /// Обработка взаимодействия игрока с NPC:
    /// 1. Пытается выдать награду за текущий квест
    /// 2. Если успешно — удаляет квест и назначает следующий
    /// 3. Если нет активного квеста — пытается выдать новый
    /// </summary>
    public void Interact(IInteractor picker)
    {
        foreach (var quest in runtimeQuests)
        {
            // Проверяем, можно ли выдать награду за квест
            if (provider.TryGiveReward(quest))
            {
                // ⚠️ Потенциальный риск: Remove внутри foreach (может вызвать исключение)
                runtimeQuests.Remove(quest);

                // Назначаем следующий квест
                TryAssignNextQuest();

                return;
            }
        }

        // Если нет активного квеста — пробуем выдать новый
        if (provider.currentQuest == null)
        {
            TryAssignNextQuest();
        }
    }

    /// <summary>
    /// Назначает следующий доступный квест:
    /// - если список не пуст
    /// - если нет активного квеста
    /// Берет первый квест из списка.
    /// </summary>
    private void TryAssignNextQuest()
    {
        if (runtimeQuests.Count == 0) return;
        if (provider.currentQuest != null) return;

        provider.SetCurrentQuest(runtimeQuests[0]);

        Debug.Log("quest started");
    }

    /// <summary>
    /// Загружает состояние провайдера:
    /// - пересоздает runtime-квесты на основе сохраненных данных
    /// - восстанавливает флаг получения награды
    /// </summary>
    public void LoadState(QuestProviderState state)
    {
        runtimeQuests.Clear();

        foreach (var savedQuest in state.allQuests)
        {
            // Ищем соответствующий квест в исходном списке
            var initialQuestState = initialQuests.Find((x) => x.questSO.id == savedQuest.Key);

            if (initialQuestState != null)
            {
                // Создаем копию состояния (важно, чтобы не мутировать initialQuests)
                var questInstance = new QuestRewardState
                {
                    questSO = initialQuestState.questSO,
                    wasRewardGranted = savedQuest.Value,
                    rewards = initialQuestState.rewards
                };

                runtimeQuests.Add(questInstance);
            }
        }
    }
}