using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestRepairEffect
{
    public QuestSO targetQuestSO;
    public RepairableConstructionSO targetConstructionSO;
}

/// <summary>
/// Применяет эффекты восстановления хаба при завершении квестов:
/// сопоставляет квест с объектом строительства и вызывает его Repair.
/// </summary>
public class HubRestorationController : QuestCompletionObserver
{
    public List<QuestRepairEffect> repairEffects = new List<QuestRepairEffect>();
    public RepairableConstructionsManager constructionsManager;
  

    protected override void React(QuestSO questSO)
    {
        
     
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
