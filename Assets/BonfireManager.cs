using System.Collections.Generic;
using UnityEngine;

public class BonfireManager : MonoBehaviour
{

    public List<Bonfire> bonfires = new List<Bonfire>(); 
    PlayerManager playerManager;    

    public void Init()
    {
        bonfires.Clear();   
        bonfires.AddRange(GetComponentsInChildren<Bonfire>());
        playerManager = FindAnyObjectByType<PlayerManager>();

        foreach (var item in bonfires)
        {
            item.Init();    
        }

        if(playerManager == null)
        {
            Debug.Log("no player manager found");
        }
    }

    public void FastTravel(string bonfireId)
    {
        var match = bonfires.Find((x)=>x.id == bonfireId);  

        if (match != null && playerManager !=null)
        {
            playerManager.serviceLocator.lifecycle.SetStartingPosition(match.GetRespawnPosition());
            playerManager.serviceLocator.lifecycle.Respawn();
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
