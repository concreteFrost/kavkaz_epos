using TMPro;

using UnityEngine;


public class LevelInfoUI : MonoBehaviour
{
  

    #region Level Statistics 
    [Header("Level Stats")]
    [SerializeField] GameObject statisticsWrapper;

    [SerializeField] TextMeshProUGUI text_levelName;
    [SerializeField] TextMeshProUGUI text_staticLoot;
    [SerializeField] TextMeshProUGUI text_bonfires;
    [SerializeField] TextMeshProUGUI text_enemies;

    LootManager lootManager;
    BonfireManager bonfireManager;
    CharactersManager characterManager;
    #endregion

    private void Awake()
    {
        characterManager = FindAnyObjectByType<CharactersManager>();
        lootManager = FindAnyObjectByType<LootManager>();
        bonfireManager = FindAnyObjectByType<BonfireManager>();
    }

    private void OnEnable()
    {
        CharactersManager.CharacterStatesUpdated += OnEnemiesInfoUpdated;
        BonfireManager.BonfireStatesUpdated += OnBonfiresInfoUpdated;
        LootManager.StaticLootDataUpdated += OnStaticLootInfoUpdated;
        LevelManager.LevelInfoUpdated += OnLevelStateUpdated;

    }

    private void OnDisable()
    {
        CharactersManager.CharacterStatesUpdated -= OnEnemiesInfoUpdated; 
        BonfireManager.BonfireStatesUpdated -= OnBonfiresInfoUpdated;
        LootManager.StaticLootDataUpdated -= OnStaticLootInfoUpdated;
        LevelManager.LevelInfoUpdated -= OnLevelStateUpdated;

    }


    #region Level Statistics

    private void ShowLevelStatistics(bool show)=>  statisticsWrapper.SetActive(show);
    private void GetUpdatedInfo(TextMeshProUGUI text, int current, int total) => text.text = $"{current}/{total}";

    public void OnStaticLootInfoUpdated()
    {
        
        int collectedLoot = 0;
        int allLoot = lootManager.loots.Count;
        foreach (var loot in lootManager.loots)
        {
            if (loot.HasInteracted) collectedLoot++;
        }


        GetUpdatedInfo(text_staticLoot, collectedLoot, allLoot);
    }

    public void OnBonfiresInfoUpdated() 
    {
        int discovered = 0;
        int allBonfires = bonfireManager.bonfires.Count;
        foreach (var bonfire in bonfireManager.bonfires)
        {
            if (bonfire.isDiscovered) discovered++;
        }
        GetUpdatedInfo(text_bonfires, discovered, allBonfires); 
    }



    public void OnEnemiesInfoUpdated()
    {
        
        int enemiesCount = characterManager.enemies.Count;
        int enemiesKilled = 0;
        foreach (var enemy in characterManager.enemies)
        {
            if (enemy.statsManager.Health.Current <= 0) enemiesKilled++;
        }

        GetUpdatedInfo(text_enemies,enemiesKilled, enemiesCount);

    }


    public void OnLevelStateUpdated(string levelName)
    {
        text_levelName.text = levelName;
    }
    #endregion

   


}
