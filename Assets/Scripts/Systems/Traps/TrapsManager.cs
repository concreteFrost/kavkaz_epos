using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class TrapsManager : MonoBehaviour
{
    public List<BaseTrap> traps = new List<BaseTrap> ();   


    public void Init()
    {

        traps = GetComponentsInChildren<BaseTrap>().ToList();    

        foreach (BaseTrap trap in traps)
        {
            trap.Init();
        }

        //Debug.Log("init traps");
    }


    public List<TrapState> SaveTrapState()
    {
        List<TrapState > result = new List<TrapState>();

        foreach (BaseTrap trap in traps)
        {
            result.Add(new TrapState
            {
                id = trap.uid,
                wasActivated = trap.wasActivated
            });

            
        }

        return result;  
    }

    public void LoadTrapsData(LevelState state)
    {
        var trapsState = state.trapStates;

        foreach (var trap in traps)
        {
            var match = state.trapStates.Find((x) => x.id == trap.uid);

            if(match != null)
            {
                //Debug.Log("found match in traps");
                trap.LoadState(match.wasActivated);
            }
        }
    }

    internal void ResetTraps()
    {
        foreach(var trap in traps)
        {
            trap.ResetState();
        }
    }
}
