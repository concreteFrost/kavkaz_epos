using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStartType
{
    None = 0,
    NewGame = 1,
    LoadGame = 2,
}
public class GameStartContext : MonoBehaviour
{
    public static GameStartContext Instance;
    public GameStartType gameStartType = GameStartType.NewGame;
    string sceneName;

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

    }

    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        ResolveGameStart();
    }

    private void ResolveGameStart()
    {
        if (gameStartType == GameStartType.None)
            GameRunner.Instance.OnNewGameStarted(); 

        if (gameStartType == GameStartType.NewGame)
            SceneTransitionManager.Instance.StartNewGame(sceneName);

        if (gameStartType == GameStartType.LoadGame)
        {
            SceneTransitionManager.Instance.LoadGame();
        }

      
    }

    public void SetGameStartType(GameStartType startType)
    {
        gameStartType = startType;
    }


}
