using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public PlayerState playerState;
    [SerializeField] private PlayerServiceLocator serviceLocator; 

    public void Init()
    {
        playerState = new PlayerState();
        serviceLocator.Init();
    }

    public PlayerState SavePlayer()
    {
        Vector3 position = serviceLocator.transform.position;
        playerState.playerPosition[0] = position.x;
        playerState.playerPosition[1] = position.y;
        playerState.playerPosition[2] = position.z;

        playerState.statsData = serviceLocator.stats.SaveStatsData();
        playerState.levelData = serviceLocator.levelController.SaveLevelData();
        playerState.effectData = serviceLocator.statsModifier.SaveEffectData();

        return playerState;
    }

    public void LoadState(PlayerState state)
    {
        Vector3 position = new Vector3(state.playerPosition[0], state.playerPosition[1], state.playerPosition[2]);
        serviceLocator.transform.position = position;
        serviceLocator.stats.LoadStatsData(state.statsData);
        serviceLocator.levelController.LoadLevelData(state.levelData);
        serviceLocator.statsModifier.LoadEffectsData(state.effectData);
    }
}
