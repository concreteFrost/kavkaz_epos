using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelState levelState; 

    [SerializeField] LootManager lootManager;
    //[SerializeField] WeaponsManager weaponsManager;
    [SerializeField] CharactersManager charactersManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Init()
    {
        levelState = new LevelState();
        levelState.levelId = SceneManager.GetActiveScene().name;

        lootManager.Init();    
        charactersManager.Init();
        //weaponsManager.Init();
    }

    public void ResetCharacters()
    {
         charactersManager.RespawnAllCharacters();  
    }

    public LevelState SaveLevelState()
    {
        levelState.lootDatas = lootManager.SaveLootData();
        levelState.dynamicLootDatas = lootManager.SaveDynamicLoot();
        levelState.enemyDatas = charactersManager.SaveEnemies();
        
        //levelState.combatItemDatas = weaponsManager.SaveCombatItemData();
        return levelState;

    }

    public void LoadLevelState(LevelState state)
    {
        levelState = state;

        lootManager.LoadLootData(state);
        lootManager.LoadDynamicLoot(state);
        charactersManager.LoadCharactersData(state);
        //weaponsManager.LoadItemData(state.combatItemDatas); 
    }
    
}
