using System.Collections.Generic;
using UnityEngine;

public abstract class BaseQuestProviderController : MonoBehaviour, IInteractable
{
    private QuestProvider provider;

    public List<QuestRewardState> initialQuests = new List<QuestRewardState>();
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
        runtimeQuests.Clear();
        runtimeQuests.AddRange(initialQuests);

        provider = new QuestProvider();
    }

    public void Interact(IInteractor picker)
    {
        foreach (var quest in runtimeQuests)
        {
            if (provider.TryGiveReward(quest))
            {
                runtimeQuests.Remove(quest);
                TryAssignNextQuest();   // ← ВОТ ЗДЕСЬ следующий квест

                return;
            }
        }

        // если нет активного квеста — дать новый
        if (provider.currentQuest == null)
        {
            TryAssignNextQuest();
        }
    }



    private void TryAssignNextQuest()
    {
        if (runtimeQuests.Count == 0) return;
        if (provider.currentQuest != null) return;

        provider.SetCurrentQuest(runtimeQuests[0]);
        Debug.Log("quest started");
    }

}




