using System;
using UnityEngine;

[System.Serializable]
public class RepairableState
{
    public string id;
    public bool isRepaired;
}

public class RepairableConstruction : MonoBehaviour, IRepairable
{
    public RepairableState state;
    
    [SerializeField] private RepairableConstructionSO repairableSO;
    [SerializeField] GameObject visual;

    public void Init()
    {
        state = new RepairableState();
        state.id = repairableSO.id;

        Break();
       
    }

    public void Repair()
    {
        visual.SetActive(true);
        state.isRepaired = true;
    }

    public void Break()
    {
        visual.SetActive(false);
        state.isRepaired = false;
    }

    internal void LoadData(RepairableState construction)
    {

        if (construction.isRepaired)
        {
            Repair();
            return;
        }

        Break();
    }
}
