using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorsManager : MonoBehaviour
{
    public List<Door> doors = new List<Door>();
    
    public void Init()
    {
        doors.Clear();
        doors = GetComponentsInChildren<Door>().ToList();

        foreach(var door in doors)
        {
            door.Init();
        }

    }

    public List<DoorState> SaveDoorsState()
    {
        List<DoorState> doorsState = new List<DoorState>();

        foreach(var door in doors)
        {
            DoorState state = door.SaveDoorState();

            doorsState.Add(state);
        }

        return doorsState;
    }

    public void LoadDoorsState(LevelState levelState)
    {
        var doorsState = levelState.doorsState;

        foreach(var state in doorsState)
        {
            var match = doors.Find((x) => x.id == state.id);

            if(match != null)
            {
                match.LoadState(state);
            }
        }
    }
}
