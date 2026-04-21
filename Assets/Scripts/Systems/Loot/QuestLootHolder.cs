using UnityEngine;

public class QuestLootHolder : StaticLootHolder
{
    QuestCompletionTrigger trigger;
    LabelQuestItem questLabel;

    public override void Interact(IInteractor collector)
    {
        base.Interact(collector);
        trigger.Trigger();
        questLabel?.ToggleText(false);
    }

    public override void Init()
    {
        base.Init();
        trigger = GetComponent<QuestCompletionTrigger>();  
        questLabel = GetComponent<LabelQuestItem>();

        questLabel?.SetText(trigger.GetQuest().questName);
        questLabel?.ToggleText(true);
    }
}

