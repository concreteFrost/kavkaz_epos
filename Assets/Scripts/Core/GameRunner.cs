using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class GameRunner : MonoBehaviour
{
    public static GameRunner Instance;
    public GameObject playerPrefab;
    public PlayerManager Player { get; private set; }
    [HideInInspector] public LevelManager activeLevel;

    private WorldStateManager worldStateManager = new WorldStateManager();

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
    }

    private void OnEnable()
    {
        SaveLoadManager.MenuLoaded += OnMenuLoaded;
        SaveLoadManager.NewGameStarted += OnNewGameStarted;
        SaveLoadManager.TravelStarted += OnTravelStarted;
        SaveLoadManager.SceneLoadedAfterTravel += OnSceneLoadedAfterTravel;
        SaveLoadManager.SaveLoaded += OnSaveLoaded;
        SaveLoadManager.GameSaved += OnGameSave;
    }

    private void OnDisable()
    {
        SaveLoadManager.MenuLoaded -= OnMenuLoaded; 
        SaveLoadManager.NewGameStarted -= OnNewGameStarted;
        SaveLoadManager.TravelStarted -= OnTravelStarted;
        SaveLoadManager.SceneLoadedAfterTravel -= OnSceneLoadedAfterTravel;
        SaveLoadManager.SaveLoaded -= OnSaveLoaded;
        SaveLoadManager.GameSaved -= OnGameSave;
    }

    public void Bootstrap()
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

        // Если нет ни глобального, ни на сцене — создаём prefab
        Player = Instantiate(playerPrefab).GetComponent<PlayerManager>();
        Player.Init();
        DontDestroyOnLoad(Player);
    }

    public void BootstrapLevel()
    {
        activeLevel = FindAnyObjectByType<LevelManager>();

    }

    public void SpawnAtLevelStart()
    {
        Vector3 pos = activeLevel.startingPosition.position;

        Player.serviceLocator.transform.position = pos;
        Player.serviceLocator.lifecycle.respawnPosition = pos;
    }

    public void OnMenuLoaded()
    {
        var scenePlayer = FindAnyObjectByType<PlayerManager>();
        if (scenePlayer != null)
        {
            Destroy(scenePlayer.gameObject);
        }
    }

    public void OnTravelStarted()
    {
        worldStateManager.SaveLevel(activeLevel);
    }

    public void OnNewGameStarted()
    {
       
        Bootstrap();
        SpawnAtLevelStart();

       
    }

    public void OnSceneLoadedAfterTravel(string sceneName)
    {
        BootstrapLevel();
        worldStateManager.LoadLevel(activeLevel);
        SpawnAtLevelStart();

        SaveLoadManager.Instance.SaveGame();
    }

    public void OnSaveLoaded(SaveGameData data)
    {
        BootstrapPlayer();
        Player.LoadState(data.playerState);

        BootstrapLevel();
        worldStateManager.LoadFromSaveData(data.levelDatas);

        worldStateManager.LoadLevel(activeLevel);
    }

    public void OnGameSave()
    {
        worldStateManager.SaveLevel(activeLevel);

        SaveGameData saveGameData = new SaveGameData();
        saveGameData.playerState = Player.SavePlayer();
        saveGameData.levelDatas = worldStateManager.GetSaveData();
        saveGameData.currentLevelName = activeLevel.GetLevelName();

        SaveLoadSystem.SaveGameData(saveGameData);
    }

}