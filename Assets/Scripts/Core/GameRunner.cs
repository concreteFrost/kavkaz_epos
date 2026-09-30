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

    public void ClearInstances()
    {
        if (playerCameraManager != null)
        {
            Destroy(playerCameraManager.gameObject);
            playerCameraManager = null;
        }

        if (Player != null)
        {
            Destroy(Player.gameObject);
            Player = null;
        }

        activeLevel = null;
    }
   
    private void BootstrapPlayer()
    {

        ClearInstances();

        // Если нет ни глобального, ни на сцене — создаём prefab
        Player = Instantiate(playerPrefab).GetComponent<PlayerManager>();
        Player.Init();
        DontDestroyOnLoad(Player.gameObject);

        playerCameraManager = Instantiate(cameraPrefab).GetComponent<PlayerCameraManager>();
        playerCameraManager.ResetCameraPosition();
        playerCameraManager.AttachCameraToPlayer(Player.serviceLocator.cameraFollow);

        DontDestroyOnLoad(playerCameraManager.gameObject);

    }

    public void BootstrapLevel()
    {
        activeLevel = null;

        activeLevel = FindAnyObjectByType<LevelManager>();

        if(activeLevel != null)
        {
            activeLevel.Init();
            GlobalAudioManager.Instance.PlayMusic(activeLevel.BiomMusic());
        }

    }

    public void Bootstrap()
    {

        BootstrapPlayer();
        BootstrapLevel();

    }

    public void StartNewGame()
    {
        Bootstrap();

        if (activeLevel != null)
            Player.serviceLocator.lifecycle.Respawn(activeLevel.GetStartingPosition());
    }

    private void OnMenuLoaded() => ClearInstances();


    public void OnTransitionStarted(float transition)
    {
        //GlobalAudioManager.Instance.StopMusic(activeLevel.BiomMusicInstance());
        worldStateManager.SaveLevel(activeLevel);
        GlobalAudioManager.Instance.StopMusic();
     
    }

    private void OnNewGameStarted() => StartNewGame();


    public void OnSceneLoadedAfterTravel(string sceneName, Vector3 startingPosition)
    {
        
        BootstrapLevel();

        worldStateManager.LoadLevel(activeLevel);
        activeLevel.ReloadWholeLevelState();

        if (startingPosition == Vector3.zero)
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
        GlobalQuestManager.Instance.LoadQuestsData(data);

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
        saveGameData.questsStates = GlobalQuestManager.Instance.SaveQuestsState();

        SaveLoadSystem.SaveGameData(saveGameData);
    }


}

