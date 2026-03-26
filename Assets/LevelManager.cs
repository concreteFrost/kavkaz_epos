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

    public LevelState SaveLevelState()
    {
        levelState.lootDatas = lootManager.SaveLootData(); 
        //levelState.combatItemDatas = weaponsManager.SaveCombatItemData();
        return levelState;

    }

    public void LoadLevelState(LevelState state)
    {
        levelState = state;

        lootManager.LoadLootData(state.lootDatas);
        //weaponsManager.LoadItemData(state.combatItemDatas); 
    }
    
}
