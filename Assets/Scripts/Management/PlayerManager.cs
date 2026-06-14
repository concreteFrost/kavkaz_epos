using UnityEngine;


public class PlayerManager : MonoBehaviour
{

    public PlayerState playerState;
    public PlayerServiceLocator serviceLocator;

    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameObject);
    //}

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

        Vector3 respawnPosition = serviceLocator.lifecycle.respawnPosition;

        playerState.respawnPosition[0] = respawnPosition.x;
        playerState.respawnPosition[1] = respawnPosition.y;
        playerState.respawnPosition[2] = respawnPosition.z;

        playerState.statsData = serviceLocator.stats.SaveStatsData();
        playerState.levelData = serviceLocator.levelController.SaveLevelData();
        playerState.effectData = serviceLocator.statsModifier.SaveEffectData();
        playerState.consumableInventoryData = serviceLocator.consumableInventory.SaveInventoryData();
        playerState.spellInventoryData = serviceLocator.spellInventory.SaveInventoryData();
        playerState.weaponsData = serviceLocator.weaponInventory.SaveInventoryData();

        playerState.moneyAmount = serviceLocator.moneyManager.CurrentBalance;

        return playerState;
    }

    public void LoadState(PlayerState state)
    {
        Vector3 position = new Vector3(state.playerPosition[0], state.playerPosition[1], state.playerPosition[2]);
        serviceLocator.transform.position = position;
        serviceLocator.stats.LoadStatsData(state.statsData);
        serviceLocator.levelController.LoadLevelData(state.levelData);
        serviceLocator.statsModifier.LoadEffectsData(state.effectData);
        serviceLocator.consumableInventory.LoadInventoryData(state.consumableInventoryData);
        serviceLocator.spellInventory.LoadInventoryData(state.spellInventoryData);
        serviceLocator.weaponInventory.LoadInventoryData(state.weaponsData);
        serviceLocator.questItemsInventory.LoadInventoryData(state.questItemsData);
        serviceLocator.moneyManager.LoadData(state.moneyAmount);

        Vector3 respawnPosition = new Vector3(state.respawnPosition[0], state.respawnPosition[1], state.respawnPosition[2]);
        serviceLocator.lifecycle.SetStartingPosition(respawnPosition);
    }
}