using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public static Action<float> TransitionStarted;
    public static Action<float> TransitionFinished;
    public static Action NewGameStarted;
    public static Action<string, Vector3> SceneLoadedAfterTravel;
    public static Action<SaveGameData> SaveLoaded;
    public static Action GameSaved;
    public static Action MenuLoaded;
    public static Action LevelLoaded;

    private float transitionTime = 2f;

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

    public void TravelToLevel(string sceneName, Vector3 startingPos)
    {
        StartCoroutine(StartTransition(sceneName,startingPos));
    }

    private void LoadSceneAfterTravel(string sceneName, Vector3 startingPos)
    {
        StartCoroutine(EndTransition(sceneName,startingPos));   
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

    private IEnumerator StartTransition( string sceneName, Vector3 startingPos) {

        GameStateManager.Instance.SetState(GameState.Transition);
        TransitionStarted?.Invoke(transitionTime);
        yield return new WaitForSeconds(transitionTime);
        yield return LoadSceneAsync(sceneName, () => LoadSceneAfterTravel(sceneName,startingPos));
       
    }

    private IEnumerator EndTransition(string sceneName, Vector3 startingPosition)
    {
        SceneLoadedAfterTravel?.Invoke(sceneName, startingPosition);
       
        yield return new WaitForSeconds(transitionTime);
       
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
        TransitionFinished?.Invoke(transitionTime);
        onLoaded?.Invoke();
        GameStateManager.Instance.SetState(GameState.Game);
    }
}