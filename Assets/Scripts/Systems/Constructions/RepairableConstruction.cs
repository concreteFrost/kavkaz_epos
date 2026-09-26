using System.Collections;
using UnityEngine;

[System.Serializable]
public class BuildingState
{
    public string id;
    public bool isRepaired;
}

public class RepairableConstruction : QuestCompletionObserver, IRepairable
{

    [SerializeField] QuestSO targetQuestSO;

    [SerializeField] private RepairableConstructionSO repairableSO;
    [SerializeField] GameObject visual;

    [HideInInspector] public string id;
    private bool isRepaired;

    public void Init()
    {
        id = GetComponent<UniqueId>().uniqueId;
        Break();

        if (GlobalQuestManager.Instance.IsQuestCompleted(targetQuestSO))
        {
           
            Repair();
        }
    }

    public void Repair()
    {
        visual.SetActive(true);
        isRepaired = true;

    }

    public void Break()
    {
        visual.SetActive(false);
        isRepaired = false;
    }

    protected override void React(QuestSO questSO)
    {
        if(questSO.id == targetQuestSO.id)
        {
            Repair();
        }
    }

    public BuildingState SaveState()
    {
        return new BuildingState()
        {
            id = id,
            isRepaired = isRepaired
        };
    }

    public void LoadData(bool isRepaired)
    {
        //this.isRepaired = isRepaired;

        //if (isRepaired)
        //{
        //    Repair();
        //}
        //else
        //{
        //    Break();
        //}
    }
}

