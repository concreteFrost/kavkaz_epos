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

    public List<BuildingState> SaveBuildingsState()
    {
        List<BuildingState> states = new List<BuildingState>();
        foreach(var b in constructions)
        {
            BuildingState data = b.SaveState();

            states.Add(data);
        }

        return states;
    }

    public void LoadState(HubState hubState)
    {
        var buildings = hubState.buildingStates;

        foreach(var state in buildings)
        {
            var match = constructions.Find((x) => x.id == state.id);

            if (match != null)
            {
                match.LoadData(state.isRepaired);
            }
        }
    }



}
