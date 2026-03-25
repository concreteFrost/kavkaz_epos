using UnityEngine;
using UnityEngine.SceneManagement;

public static class PerformBootstrap
{
    const string SceneName = "TestScene";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        for(int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; ++sceneIndex)
        {
            var candidate = SceneManager.GetSceneAt(sceneIndex);

            if (candidate.name == SceneName) return;
        }

        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
    }

}
public class GameBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);    
    }
}