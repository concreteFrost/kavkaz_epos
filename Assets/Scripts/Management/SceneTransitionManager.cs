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

    Coroutine loadGameCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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

    public void StartNewGame(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName, () =>
        {
           NewGameStarted?.Invoke();
        }, GameState.Game));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(TransitionToScene("MainMenu", () =>
        {
           MenuLoaded?.Invoke();
        }, GameState.Menu));
    }

    public void SaveGame()
    {
        GameSaved?.Invoke();
    }

    public void LoadGame()
    {
        if(loadGameCoroutine == null)
        {
            loadGameCoroutine = StartCoroutine(LoadGameCoroutine());
        }
       
    }

    public void TravelToLevel(string sceneName, Vector3 startingPos)
    {
        StartCoroutine(TransitionToScene(sceneName, () =>
        {
            SceneLoadedAfterTravel?.Invoke(sceneName, startingPos);
        }, GameState.Game));
    }


    private IEnumerator LoadGameCoroutine()
    {
        SaveGameData data = SaveLoadSystem.LoadGameData();

        yield return TransitionToScene(data.currentLevelName, () =>
        {
            SaveLoaded?.Invoke(data);
        }, GameState.Game);

        loadGameCoroutine = null;

    }


    private IEnumerator TransitionToScene(string sceneName, Action onLoaded, GameState state)
    {
        GameStateManager.Instance.SetState(GameState.Transition);

        TransitionStarted?.Invoke(transitionTime);
        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
                asyncLoad.allowSceneActivation = true;

            yield return null;
        }

        onLoaded?.Invoke();

        //yield return new WaitForSeconds(transitionTime);
        yield return new WaitForSeconds(transitionTime);
        TransitionFinished?.Invoke(transitionTime);

        GameStateManager.Instance.SetState(state);


    }
}