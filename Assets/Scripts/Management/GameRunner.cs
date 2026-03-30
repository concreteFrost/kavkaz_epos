using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRunner : MonoBehaviour
{
    public static GameRunner Instance;
    public GameObject playerPrefab;
    public PlayerManager Player { get; private set; }

    Dictionary<string, LevelState> allLevels = new Dictionary<string, LevelState>();
    LevelManager activeLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
     
        Bootstrap();
        SpawnAtLevelStart();

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

    private void Bootstrap()
    {
        BootstrapPlayer();
        BootstrapLevel();
    }

    private void BootstrapPlayer()
    {

        var scenePlayer = FindAnyObjectByType<PlayerManager>();
        if (scenePlayer != null)
        {
            Destroy(scenePlayer.gameObject);    
        }

        // 3. Если нет ни глобального, ни на сцене — создаём prefab
        Player = Instantiate(playerPrefab).GetComponent<PlayerManager>();
        Player.Init();
    }

    private void BootstrapLevel()
    {
        activeLevel = FindAnyObjectByType<LevelManager>();

        if (activeLevel != null)
        {
            activeLevel.Init();
        }
    }


    private void SpawnAtLevelStart()
    {
        Vector3 pos = activeLevel.startingPosition.position;

        Player.serviceLocator.transform.position = pos;
        Player.serviceLocator.lifecycle.respawnPosition = pos;
    }


    public void TravelToLevel(string sceneName)
    {
        // 1. Сохраняем текущий уровень
        if (activeLevel != null)
        {
            LevelState currentState = activeLevel.SaveLevelState();
            allLevels[currentState.levelId] = currentState;
        }

        // 2. Подписываемся на загрузку сцены
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 3. Загружаем сцену
        SceneManager.LoadScene(sceneName);

    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 4. Находим и инициализируем новый уровень
        activeLevel = FindAnyObjectByType<LevelManager>();
        activeLevel.Init();

        // 5. Загружаем состояние уровня (если есть)
        if (allLevels.TryGetValue(scene.name, out LevelState state))
        {
            activeLevel.LoadLevelState(state);
            activeLevel.ReloadWholeLevelState();
        }

        SpawnAtLevelStart();
        
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
        saveGameData.playerState = Player.SavePlayer();

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

        saveGameData.currentLevelName = activeLevel.GetLevelName();

        // Сохраняем на диск
        SaveLoadManager.SaveGameData(saveGameData);

    }

    public void LoadGame()
    {
        SaveGameData loadedData = SaveLoadManager.LoadGameData();
        if (loadedData == null) return;

        // Загружаем игрока
        Player.LoadState(loadedData.playerState);


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
