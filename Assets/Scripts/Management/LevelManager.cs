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
    [SerializeField] LevelInfoSO levelInfoSO;

    [HideInInspector] public LevelState levelState;
    public Transform startingPosition;

    [SerializeField] private LootManager lootManager;
    [SerializeField] private CharactersManager charactersManager;
    [SerializeField] private BonfireManager bonfireManager;
    [SerializeField] private BossesManager bossesManager;

    public static Action<string> LevelInfoUpdated;

    private void Awake()
    {
        levelState = new LevelState();

        levelState.levelId = levelInfoSO.id;

        InitSystems();

        Bonfire.BonfireInteracted += ReloadLevelOnRest;
    }

    private void Start()
    {
        LevelInfoUpdated?.Invoke(levelInfoSO.levelName);
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
        bonfireManager?.Init();
        bossesManager?.Init();
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
            levelState.enemyDatas = charactersManager.SaveEnemies();
        }

        if (bonfireManager != null)
        {
            levelState.bonfireDatas = bonfireManager.SaveBonfireStates();
        }

        if (bossesManager != null)
        {
            levelState.bossArenaStates = bossesManager.SaveBossesState();
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

        LevelInfoUpdated?.Invoke(levelInfoSO.levelName);
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (startingPosition == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(startingPosition.position, 0.5f);
    }
}