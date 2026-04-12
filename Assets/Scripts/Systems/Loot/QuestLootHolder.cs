using UnityEngine;

public class QuestLootHolder : StaticLootHolder
{
    [SerializeField] QuestCompletionTrigger trigger;

    public override void Interact(IInteractor collector)
    {
        base.Interact(collector);
        trigger.Trigger();
    }

    public override void Init()
    {
        base.Init();
        trigger = GetComponent<QuestCompletionTrigger>();   
    }
}

