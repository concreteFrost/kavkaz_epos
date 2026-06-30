using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarterMenuUI : MonoBehaviour
{
    [SerializeField] BiomInfoSO starterBiom;

    [SerializeField] Button btn_startNewGame;
    [SerializeField] Button btn_loadGame;
    [SerializeField] Button btn_quitGame;

    private List<Selectable> allBtns;

    public void Start()
    {

        btn_loadGame.interactable = SaveLoadSystem.HasSave();

        GameStateManager.Instance.SetState(GameState.Menu);

        BindActions();  

        allBtns = new List<Selectable>()
        {
            btn_startNewGame,
            btn_loadGame,
            btn_quitGame,
        };

        UINavigationUtils.ClampVerticalNavigation(allBtns);
        StartCoroutine(UINavigationUtils.SelectWithDelay(allBtns[0].gameObject));

    }

    private void OnDisable()
    {
        RemoveAllListeners();   
    }

    private void BindActions()
    {
        btn_startNewGame.onClick.AddListener(StartNewGame);
        btn_loadGame.onClick.AddListener(LoadGame);
        btn_quitGame.onClick.AddListener(QuitGame); 
    }

    private void RemoveAllListeners()
    {
        btn_startNewGame.onClick.RemoveListener(StartNewGame);
        btn_loadGame.onClick.RemoveListener(LoadGame);
        btn_quitGame.onClick.RemoveListener(QuitGame);
    }

    private void StartNewGame()
    {
        SceneTransitionManager.Instance.StartNewGame(starterBiom.biomName);
    }

    private void LoadGame()
    {
        SceneTransitionManager.Instance.LoadGame();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
