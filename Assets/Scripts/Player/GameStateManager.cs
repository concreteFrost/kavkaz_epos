using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    Game = 0,
    Inventory = 1,
    Transition = 2,
    Menu= 3,
    Bonfire = 4,
    Dialogue= 5,

}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public static Action<GameState> GameStateChanged;
    public GameState CurrentState;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
 
    }

    //private void Start()
    //{
    //    GameStateChanged?.Invoke(GameState.Game);
    //}

    public void SetState(GameState newState)
    {
        //if (CurrentState == newState) return;
      
        CurrentState = newState;
        SetCursorState(newState);
        GameStateChanged?.Invoke(CurrentState);
    }

    private void SetCursorState(GameState newState)
    {
        bool isCursorActive = newState != GameState.Game && newState != GameState.Transition;
        Cursor.visible = isCursorActive;
        Cursor.lockState = isCursorActive ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

}