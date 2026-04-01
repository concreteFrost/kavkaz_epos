using TMPro;
using UnityEngine;

public class LevelInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_levelName;
    [SerializeField] TextMeshProUGUI text_staticLoot;
    [SerializeField] TextMeshProUGUI text_dynamicLoot;
    [SerializeField] TextMeshProUGUI text_bonfires;
    [SerializeField] TextMeshProUGUI text_enemies;

    private void OnEnable()
    {
        LevelManager.LevelInfoUpdated += OnLevelStateUpdated;
    }

    private void OnDisable()
    {
        LevelManager.LevelInfoUpdated -= OnLevelStateUpdated;   
    }

    public void GetLevelName(string name)=> text_levelName.text = name;

    public void GetUpdatedInfo(TextMeshProUGUI text, int current,int total) => text.text = $"{current}/{total}";

    public void GetEnemiesInfo(int current, int total) => GetUpdatedInfo( text_enemies,current, total);
    public void GetStaticLootInfo(int current, int total)=>GetUpdatedInfo(text_staticLoot,current,total);
    public void GetDynamicLootInfo(int total) => text_dynamicLoot.text = total.ToString();
    public void GetBonfiresInfo(int current,int total) => GetUpdatedInfo(text_bonfires,current,total); 

    public void OnLevelStateUpdated(LevelState state)
    {
        GetLevelName(state.levelId);

        int enemiesCount = state.enemyDatas.Count;
        int enemiesKilled = 0;
        foreach (var enemy in state.enemyDatas)
        {
            if (enemy.statsData.currentHealth <= 0) enemiesKilled++;
        }

        GetEnemiesInfo(enemiesKilled, enemiesCount);

        int staticLootCount = state.staticLootDatas.Count; ;
        int collectedLootCount = 0;

        foreach(var staticLoot in state.staticLootDatas)
        {
            if(staticLoot.hasCollected) collectedLootCount++;   
        }

        GetStaticLootInfo(collectedLootCount, staticLootCount);

        int dynamicLootCount = state.dynamicLootDatas.Count;
        GetDynamicLootInfo(dynamicLootCount);

        int bonfiresCount = state.bonfireDatas.Count;
        int discoveredBonfires = 0;

        foreach(var bonfire in state.bonfireDatas)
        {
            if(bonfire.isDiscovered) discoveredBonfires++;  
        }

        GetBonfiresInfo(discoveredBonfires, bonfiresCount); 

    }
    

    
}
