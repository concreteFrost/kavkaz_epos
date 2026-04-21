using UnityEngine;

public class QuestCompletionTrigger : MonoBehaviour
{
    [SerializeField] private QuestSO targetQuest;

    public QuestSO GetQuest() => targetQuest;

    public void Trigger()
    {
        GlobalQuestManager.Instance.CompleteQuest(targetQuest);
    }
}


