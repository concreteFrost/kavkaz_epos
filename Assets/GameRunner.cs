using System.Collections.Generic;
using UnityEngine;
using Zenject.SpaceFighter;

public class GameRunner : MonoBehaviour
{
    public static GameRunner Instance;

    PlayerManager playerManager;
    Dictionary<string,LevelState> allLevels = new Dictionary<string,LevelState>();  
    LevelManager activeLevel;
   
    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);    
        }

        playerManager = FindAnyObjectByType<PlayerManager>();
        playerManager.Init();

        activeLevel = FindAnyObjectByType<LevelManager>();
        activeLevel.Init(); 

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        if (activeLevel != null)
        {
            // Сохраняем текущий уровень в словарь
            LevelState currentState = activeLevel.SaveLevelState();
            allLevels[currentState.levelId] = currentState;
        }

        // Создаём объект для сериализации
        SaveGameData saveGameData = new SaveGameData();
        saveGameData.playerState = playerManager.SavePlayer();

        // Переносим все уровни в List<SaveLevelData>
        saveGameData.levelDatas = new List<SaveLevelData>();
        foreach (var kv in allLevels)
        {
            saveGameData.levelDatas.Add(new SaveLevelData
            {
                levelName = kv.Key,
                levelState = kv.Value
            });
        }

        // Сохраняем на диск
        SaveLoadManager.SaveGameData(saveGameData);
        
    }

    public void LoadGame()
    {
        SaveGameData loadedData = SaveLoadManager.LoadGameData();
        if (loadedData == null) return;

        // Загружаем игрока
        playerManager.LoadState(loadedData.playerState);
        

        // Загружаем словарь всех уровней
        allLevels.Clear();
        foreach (var levelData in loadedData.levelDatas)
        {
            allLevels[levelData.levelName] = levelData.levelState;
        }

        // Восстанавливаем состояние текущего уровня
        string currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (allLevels.TryGetValue(currentLevel, out LevelState currentState))
        {
            activeLevel.LoadLevelState(currentState);
        }

        Debug.Log("Game loaded successfully");
    }


}
