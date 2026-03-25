using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelState levelState; 

    [SerializeField] LootManager lootManager;
    [SerializeField] WeaponsManager weaponsManager;
    [SerializeField] CharactersManager charactersManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Init()
    {
        levelState = new LevelState();
        levelState.levelId = SceneManager.GetActiveScene().name;

        lootManager.Init();
        weaponsManager.Init();
        charactersManager.Init();
    }

    public LevelState SaveLevelState()
    {
        levelState.lootData = lootManager.SaveLootData(); 

        return levelState;

    }

    public void LoadLevelState(LevelState state)
    {
        levelState = state;

        lootManager.LoadLootData(state.lootData);
    }
    
}
