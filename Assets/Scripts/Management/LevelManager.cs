using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveLevelData
{
    public string levelName;
    public LevelState levelState;
}


public class LevelManager : MonoBehaviour
{
    public LevelState levelState;

    public Transform startingPosition;

    [SerializeField] LootManager lootManager;
    //[SerializeField] WeaponsManager weaponsManager;
    [SerializeField] CharactersManager charactersManager;
    [SerializeField] BonfireManager bonfireManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    private void Awake()
    {
        levelState = new LevelState();
        levelState.levelId = SceneManager.GetActiveScene().name;
        
        if(lootManager == null) lootManager = FindAnyObjectByType<LootManager>();   
        if(charactersManager == null) charactersManager = FindAnyObjectByType<CharactersManager>();
        if(bonfireManager == null) bonfireManager = FindAnyObjectByType<BonfireManager>();  

        lootManager?.Init();    
        charactersManager?.Init();
        bonfireManager?.Init();  

        Bonfire.BonfireInteracted += ReloadLevelOnRest;
       
    }

    private void Start()
    {
        
    }

    private void OnDisable()
    {
        Bonfire.BonfireInteracted -= ReloadLevelOnRest; 
    }

    public string GetLevelName() => levelState.levelId;

    #region Level State Control
    public void ReloadWholeLevelState()
    {
         charactersManager.RespawnAllCharacters();
         lootManager.ClearDynamicLoot();
    }

    public void ReloadLevelOnRest()
    {
        charactersManager.RespawnAllCharacters();
    }
    #endregion


    #region Save/Load
    public LevelState SaveLevelState()
    {
        levelState.lootDatas = lootManager.SaveLootData();
        levelState.dynamicLootDatas = lootManager.SaveDynamicLoot();
        levelState.enemyDatas = charactersManager.SaveEnemies();
        levelState.bonfireDatas = bonfireManager.SaveBonfireStates();
        
        //levelState.combatItemDatas = weaponsManager.SaveCombatItemData();
        return levelState;

    }

    public void LoadLevelState(LevelState state)
    {
        levelState = state;

        lootManager.LoadLootData(state);
        lootManager.LoadDynamicLoot(state);
        charactersManager.LoadCharactersData(state);
        bonfireManager.LoadBonfireDatas(state);
        //weaponsManager.LoadItemData(state.combatItemDatas); 
    }


    #endregion

    private void OnDrawGizmos()
    {
        if (startingPosition == null) return;

        Gizmos.color = new Color(0f, 0f, 1f, 1f);
        Gizmos.DrawSphere(startingPosition.position, .5f);

    }
}
