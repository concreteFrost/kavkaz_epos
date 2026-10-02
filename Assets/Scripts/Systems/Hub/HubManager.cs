using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HubState
{
    public List<BuildingState> buildingStates = new List<BuildingState>();
}
public class HubManager : MonoBehaviour
{

    [SerializeField] RepairableConstructionsManager constructionsManager;

    public void Init()
    {

        constructionsManager?.Init();

    }

    public HubState SaveHubState()
    {

        return new HubState()
        {
            buildingStates = constructionsManager.SaveBuildingsState()
        };

    }
    
    public void LoadHubState(LevelState state)
    {

        constructionsManager.LoadState(state.hubState);


        
    }
}
