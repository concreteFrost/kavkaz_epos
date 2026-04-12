using UnityEngine;

public class QuestCompletionTrigger : MonoBehaviour
{
    [SerializeField] private QuestSO targetQuest;


    public void Trigger()
    {
        GlobalQuestManager.Instance.CompleteQuest(targetQuest);
    }
}


