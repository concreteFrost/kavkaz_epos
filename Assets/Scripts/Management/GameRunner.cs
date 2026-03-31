using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRunner : MonoBehaviour
{
    public static GameRunner Instance;
    public GameObject playerPrefab;
    public PlayerManager Player { get; private set; }

    [HideInInspector] public Dictionary<string, LevelState> allLevels = new Dictionary<string, LevelState>();
    [HideInInspector] public LevelManager activeLevel;

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

       
    }


    private void SpawnAtLevelStart()
    {
        Vector3 pos = activeLevel.startingPosition.position;

        Player.serviceLocator.transform.position = pos;
        Player.serviceLocator.lifecycle.respawnPosition = pos;
    }


  





}