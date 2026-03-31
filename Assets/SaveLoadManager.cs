using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
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

    public void TravelToLevel(string sceneName)
    {
        var activeLevel = GameRunner.Instance.activeLevel;
        var allLevels = GameRunner.Instance.allLevels;
        // 1. Сохраняем текущий уровень
        if (activeLevel != null)
        {
            LevelState currentState = activeLevel.SaveLevelState();
            allLevels[currentState.levelId] = currentState;
        }

        StartCoroutine(LoadSceneAfterTravelCoroutine(sceneName));

    }

    IEnumerator LoadSceneAfterTravelCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;
        //wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //scene has loaded as much as possible,
            // the last 10% can't be multi-threaded
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        GameRunner.Instance.BootstrapLevel();

        var allLevels = GameRunner.Instance.allLevels;
        // 5. Загружаем состояние уровня (если есть)
        if (allLevels.TryGetValue(sceneName, out LevelState state))
        {
            GameRunner.Instance.activeLevel.LoadLevelState(state);
            //activeLevel.ReloadWholeLevelState();

        }

        GameRunner.Instance.SpawnAtLevelStart();

    }

    public void LoadGame()
    {
        SaveGameData sv = SaveLoadSystem.LoadGameData();
        StartCoroutine(LoadGameCoroutine(sv));
    }

    IEnumerator LoadGameCoroutine(SaveGameData data)
    {
        var sceneName = data.currentLevelName;

        // Загружаем сцену
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        GameRunner.Instance.BootstrapLevel();
        
        var allLevels = GameRunner.Instance.allLevels;
        foreach (var levelData in data.levelDatas)
        {
            allLevels[levelData.levelName] = levelData.levelState;
        }

        if (allLevels.TryGetValue(sceneName, out LevelState state))
        {
           GameRunner.Instance.activeLevel.LoadLevelState(state);
        }

        // Если игрок ещё не создан, создаём

        GameRunner.Instance.Player.LoadState(data.playerState);

    }


    public void SaveGame()
    {
        var activeLevel = GameRunner.Instance.activeLevel;
        var allLevels = GameRunner.Instance.allLevels;
        var player = GameRunner.Instance.Player;

        if (activeLevel != null)
        {
            // Сохраняем текущий уровень в словарь
            LevelState currentState = activeLevel.SaveLevelState();
            allLevels[currentState.levelId] = currentState;
        }

        // Создаём объект для сериализации
        SaveGameData saveGameData = new SaveGameData();
        saveGameData.playerState = player.SavePlayer();

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
        SaveLoadSystem.SaveGameData(saveGameData);

    }
}
