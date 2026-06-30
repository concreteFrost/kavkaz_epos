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
    [SerializeField] private TrapsManager trapsManager;

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
        trapsManager?.Init();   
    }

    #endregion

    #region Level State Control

    public void ReloadWholeLevelState()
    {
        charactersManager?.RespawnAllCharacters();
        lootManager?.ClearDynamicLoot();
        trapsManager?.ResetTraps();
    }

    public void ReloadLevelOnRest()
    {
        trapsManager?.ResetTraps();
        charactersManager?.RespawnAllCharacters();
    }

    public Vector3 GetStartingPosition() => startingPosition.position;

    #endregion

    #region Save/Load

    public LevelState SaveLevelState()
    {
        if (lootManager != null)
        {
            levelState.staticLootStates = lootManager.SaveLootData();
            levelState.dynamicLootStated = lootManager.SaveDynamicLoot();
        }

        if (charactersManager != null)
        {
            levelState.characterStates = charactersManager.SaveCharacters();
        }

        if (bonfireManager != null)
        {
            levelState.bonfireStates = bonfireManager.SaveBonfireStates();
        }

        if (bossesManager != null)
        {
            levelState.bossArenaStates = bossesManager.SaveBossesState();
        }

        if(hubManager != null)
        {
            levelState.hubState = hubManager.SaveHubState();    
        }

        if(trapsManager != null)
        {
            levelState.trapStates = trapsManager.SaveTrapState();
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
        trapsManager?.LoadTrapsData(state); 
      
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