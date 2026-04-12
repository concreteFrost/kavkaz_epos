using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class HubState
{
    public List<RepairableState> constructions = new List<RepairableState>();
}
public class HubManager : MonoBehaviour
{

    [SerializeField] RepairableConstructionsManager constructionsManager;
    [SerializeField] HubState hubState;

    public void Init()
    {
        hubState = new HubState();
        constructionsManager?.Init();

    }

    public HubState SaveHubState()
    {
        if(constructionsManager != null)
        {
            hubState.constructions = constructionsManager.SaveConstructions();
        }

        return hubState;    
    }
    
    public void LoadHubState(LevelState state)
    {
        var loadedHubState = state.hubState;
        hubState =loadedHubState;

        if (constructionsManager != null)
            constructionsManager.LoadConstruction(loadedHubState);
    }
}
