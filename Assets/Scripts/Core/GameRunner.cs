using System;
using UnityEngine;


public class GameRunner : MonoBehaviour
{
    public static GameRunner Instance;

    public GameObject playerPrefab;

    public GameObject cameraPrefab;
    PlayerCameraManager playerCameraManager;
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
        SceneTransitionManager.MenuLoaded += OnMenuLoaded;
        SceneTransitionManager.NewGameStarted += OnNewGameStarted;
        SceneTransitionManager.TransitionStarted += OnTransitionStarted;
        SceneTransitionManager.SceneLoadedAfterTravel += OnSceneLoadedAfterTravel;
        SceneTransitionManager.SaveLoaded += OnSaveLoaded;
        SceneTransitionManager.GameSaved += OnGameSave;

    }


    private void OnDisable()
    {
        SceneTransitionManager.MenuLoaded -= OnMenuLoaded; 
        SceneTransitionManager.NewGameStarted -= OnNewGameStarted;
        SceneTransitionManager.TransitionStarted -= OnTransitionStarted;
        SceneTransitionManager.SceneLoadedAfterTravel -= OnSceneLoadedAfterTravel;
        SceneTransitionManager.SaveLoaded -= OnSaveLoaded;
        SceneTransitionManager.GameSaved -= OnGameSave;
       
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

        var camManager = FindAnyObjectByType<PlayerCameraManager>();

        if( camManager != null)
        {
            Destroy(camManager.gameObject);
        }

        playerCameraManager = Instantiate(cameraPrefab).GetComponent<PlayerCameraManager>();
        playerCameraManager.ResetCameraPosition();
        playerCameraManager.AttachCameraToPlayer(Player.serviceLocator.cameraFollow);

        DontDestroyOnLoad(playerCameraManager);    
        
    }

    public void BootstrapLevel()
    {
        activeLevel = FindAnyObjectByType<LevelManager>();
        

    }

    public void OnMenuLoaded()
    {
        var scenePlayer = FindAnyObjectByType<PlayerManager>();
        if (scenePlayer != null)
        {
            Destroy(scenePlayer.gameObject);
        }
    }

    public void OnTransitionStarted(float transition)
    {
        worldStateManager.SaveLevel(activeLevel);
    }

    public void OnNewGameStarted()
    {  
        Bootstrap();
        Player.serviceLocator.lifecycle.Respawn(activeLevel.GetStartingPosition());

    }


    public void OnSceneLoadedAfterTravel(string sceneName, Vector3 startingPosition)
    {
        BootstrapLevel();

        worldStateManager.LoadLevel(activeLevel);
        activeLevel.ReloadWholeLevelState();

        if(startingPosition == Vector3.zero)
        {
            startingPosition = activeLevel.GetStartingPosition();
        }
        Player.serviceLocator.lifecycle.Respawn(startingPosition);
        playerCameraManager.ResetCameraPosition();
       
        GlobalQuestManager.Instance.GetCurrentQuestsState();
        SceneTransitionManager.Instance.SaveGame();
    }

    public void OnSaveLoaded(SaveGameData data)
    {
        BootstrapPlayer();
        Player.LoadState(data.playerState);

        BootstrapLevel();
        worldStateManager.LoadFromSaveData(data.levelDatas);

        worldStateManager.LoadLevel(activeLevel);

        GlobalQuestManager.Instance.LoadQuestsData(data);
    }

    public void OnGameSave()
    {
        
        worldStateManager.SaveLevel(activeLevel);

        SaveGameData saveGameData = new SaveGameData();
        saveGameData.playerState = Player.SavePlayer();
        saveGameData.levelDatas = worldStateManager.GetSaveData();
        saveGameData.currentLevelName = activeLevel.GetLevelName();
        saveGameData.questsStates = GlobalQuestManager.Instance.SaveQuestsState();

        SaveLoadSystem.SaveGameData(saveGameData);
    }


}