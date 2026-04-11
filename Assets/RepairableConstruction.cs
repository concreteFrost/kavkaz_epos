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
    }

    public void Break()
    {
        visual.SetActive(false);    
    }

    internal void LoadData(RepairableState construction)
    {
        state.isRepaired = construction.isRepaired;     

        if (state.isRepaired)
        {
            Repair();
            return;
        }

        Break();
    }
}
