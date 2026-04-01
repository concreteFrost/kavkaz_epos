using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    public static Action TravelStarted;
    public static Action NewGameStarted;
    public static Action<string> SceneLoadedAfterTravel;
    public static Action<SaveGameData> SaveLoaded;
    public static Action GameSaved;
    public static Action MenuLoaded;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SaveGame();
        if (Input.GetKeyDown(KeyCode.F6)) LoadGame();
    }

    public void TravelToLevel(string sceneName)
    {
        TravelStarted?.Invoke();
        StartCoroutine(LoadSceneAsync(sceneName, () => LoadSceneAfterTravel(sceneName)));
    }

    private void LoadSceneAfterTravel(string sceneName)
    {
        SceneLoadedAfterTravel?.Invoke(sceneName);
       
    }

    public void StartNewGame(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, NewGameStarted));
    }

    public void LoadGame()
    {
        StartCoroutine(LoadGameCoroutine());
    }

    public void SaveGame()
    {
        GameSaved?.Invoke();
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneAsync("MainMenu",MenuLoaded));
    }

    private IEnumerator LoadGameCoroutine()
    {
        SaveGameData data = SaveLoadSystem.LoadGameData();
        yield return LoadSceneAsync(data.currentLevelName, () => SaveLoaded?.Invoke(data));
    }

    private IEnumerator LoadSceneAsync(string sceneName, Action onLoaded)
    {


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
                asyncLoad.allowSceneActivation = true;

            yield return null;
        }

        onLoaded?.Invoke();
    }
}