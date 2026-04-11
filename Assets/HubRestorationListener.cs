using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestRepairEffect
{
    public QuestSO targetQuestSO;
    public RepairableConstructionSO targetConstructionSO;
}

public class HubRestorationListener : QuestCompletionListener
{
    public List<QuestRepairEffect> repairEffects = new List<QuestRepairEffect>();
    public RepairableConstructionsManager constructionsManager;
  

    protected override void React(QuestSO questSO)
    {
        Debug.Log("Reacting");
     
        if (constructionsManager.constructions.Count == 0) return;


        foreach (var repair in repairEffects)
        {
            if (questSO.id == repair.targetQuestSO.id)
            {
                var match = constructionsManager.constructions.Find((x) => x.state.id == repair.targetConstructionSO.id);

                if (match != null)
                {
                    match.Repair();
                }
            }
        }
    }
}
