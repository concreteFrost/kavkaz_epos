using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RepairableConstructionsManager : MonoBehaviour
{
    public List<RepairableConstruction> constructions = new List<RepairableConstruction>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        constructions = FindObjectsByType<RepairableConstruction>(FindObjectsSortMode.None).ToList();

        foreach (var construction in constructions)
        {
            construction.Init();
        }
    }

    internal void LoadConstruction(HubState state)
    {
        var loadedConstructions = state.constructions;

        foreach (var construction in loadedConstructions)
        {
            var match = constructions.Find((x) => x.state.id == construction.id);

            if(match != null)
            {
                match.LoadData(construction);
            }
        }
    }

    internal List<RepairableState> SaveConstructions()
    {
        List<RepairableState> states = new List<RepairableState>();

        foreach (var construction in constructions)
        {
            states.Add(construction.state); 
        }

        return states;  
    }
}
