using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BonfireManager : MonoBehaviour
{

    public List<Bonfire> bonfires = new List<Bonfire>();
    public static Action<Vector3> FastTravelStarted;

    public void Init()
    {
        bonfires.Clear();
        bonfires = GetComponentsInChildren<Bonfire>().ToList();
        
        foreach (var item in bonfires)
        {
            item.Init();    
        }

    }

    public void FastTravel(string bonfireId)
    {
        var match = bonfires.Find((x)=>x.id == bonfireId);  

     

        if (match != null )
        {
            FastTravelStarted?.Invoke(match.GetRespawnPosition());  
            GameStateManager.GameStateChanged?.Invoke(GameState.Game);
        }
    }

    public List<BonfireState> SaveBonfireStates()
    {
        List<BonfireState> states = new List<BonfireState>();

        foreach (var bonfire in bonfires)
        {
            var state = new BonfireState()
            {
                isDiscovered = bonfire.isDiscovered,
                bonfireId = bonfire.id,
            };

            states.Add(state);  
        }

        return states;  
    }

    public void LoadBonfireDatas(LevelState state)
    {
        var bonfireDatas = state.bonfireDatas;

        foreach (var data in bonfireDatas)
        {
            var match = bonfires.Find((x)=>x.id == data.bonfireId);

            if (match)
            {
                match.LoadData(data);
            }
        }
       
    }

    public List<Bonfire> GetDiscoveredBonfires()
    {
        return bonfires.FindAll((x) => x.isDiscovered == true);
    }
}
