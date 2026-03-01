using System;
using UnityEngine;

public enum GameState
{
    Game = 0,
    Inventory = 1,
    Transition = 2,

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

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        GameStateChanged?.Invoke(CurrentState);
    }

}