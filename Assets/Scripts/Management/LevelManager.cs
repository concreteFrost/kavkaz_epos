using System;
using UnityEngine;

[Serializable]
public class SaveLevelData
{
    public string levelId;
    public LevelState levelState;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] BiomInfoSO biomInfoSO;
    [SerializeField]

    [HideInInspector] public LevelState levelState;
    [SerializeField] private Transform startingPosition;

    [SerializeField] private LootManager lootManager;
    [SerializeField] private CharactersManager charactersManager;
    [SerializeField] private BonfireManager bonfireManager;
    [SerializeField] private BossesManager bossesManager;
    [SerializeField] private HubManager hubManager;

    public static Action<string> LevelInfoUpdated;
    public static Action<string> LevelLoaded;

    private void Awake()
    {
        levelState = new LevelState();

        levelState.levelId = biomInfoSO.biomName;

        InitSystems();

        Bonfire.BonfireInteracted += ReloadLevelOnRest;
    }

    private void Start()
    {
        LevelInfoUpdated?.Invoke(biomInfoSO.biomName);
        LevelLoaded?.Invoke(biomInfoSO.biomName);
    }

    private void OnDisable()
    {
        Bonfire.BonfireInteracted -= ReloadLevelOnRest;
    }

    public string GetLevelName() => levelState.levelId;

    #region Init

    private void InitSystems()
    {
        lootManager?.Init();
        charactersManager?.Init();
        bonfireManager?.Init(GetLevelName());
        bossesManager?.Init();
        hubManager?.Init();  
    }

    #endregion

    #region Level State Control

    public void ReloadWholeLevelState()
    {
        charactersManager?.RespawnAllCharacters();
        lootManager?.ClearDynamicLoot();
    }

    public void ReloadLevelOnRest()
    {
        charactersManager?.RespawnAllCharacters();
    }

    public Vector3 GetStartingPosition() => startingPosition.position;

    #endregion

    #region Save/Load

    public LevelState SaveLevelState()
    {
        if (lootManager != null)
        {
            levelState.staticLootDatas = lootManager.SaveLootData();
            levelState.dynamicLootDatas = lootManager.SaveDynamicLoot();
        }

        if (charactersManager != null)
        {
            levelState.characterStates = charactersManager.SaveCharacters();
        }

        if (bonfireManager != null)
        {
            levelState.bonfireDatas = bonfireManager.SaveBonfireStates();
        }

        if (bossesManager != null)
        {
            levelState.bossArenaStates = bossesManager.SaveBossesState();
        }

        if(hubManager != null)
        {
            levelState.hubState = hubManager.SaveHubState();    
        }

        return levelState;
    }

    public void LoadLevelState(LevelState state)
    {
        if (state == null)
        {
            Debug.LogWarning("LevelState is null");
            return;
        }

        levelState = state;

        lootManager?.LoadLootData(state);
        lootManager?.LoadDynamicLoot(state);

        charactersManager?.LoadCharactersData(state);
        bonfireManager?.LoadBonfireDatas(state);
        bossesManager?.LoadBossesState(state);
        hubManager?.LoadHubState(state);
      
        LevelInfoUpdated?.Invoke(biomInfoSO.biomName);
        
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (startingPosition == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(startingPosition.position, 0.5f);
    }

}