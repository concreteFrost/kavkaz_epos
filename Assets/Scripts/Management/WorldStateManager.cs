using UnityEngine;

public class WorldStateManager : MonoBehaviour
{
    public static WorldStateManager Instance;   
    public WorldState worldState = new WorldState();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public LevelState GetLevelState(string levelId)
    {
        if (!worldState.levels.ContainsKey(levelId))
        {
            var newState = new LevelState
            {
                levelId = levelId,
            };

            worldState.levels[levelId] = newState;  
        
        }

        return worldState.levels[levelId];  

    }
}
