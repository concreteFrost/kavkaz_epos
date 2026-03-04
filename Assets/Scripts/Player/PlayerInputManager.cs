using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerGameInput gameInput;
    [SerializeField] private PlayerUIInput uiInput;
    private PlayerInputReader reader;

    public void Init(PlayerInputReader reader)
    {
        this.reader = reader;
        reader.controls.Enable();   
        GameStateManager.GameStateChanged += OnGameStateChanged;
    }


    private void OnDisable()
    {
        GameStateManager.GameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        SetGameMode(); // по умолчанию
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Game:
                SetGameMode();
                break;
            case GameState.Menu:
            case GameState.Inventory:
                SetUIMode();
                break;
            case GameState.Transition:
                DisableAll();
                break;
        }
    }

    private void SetGameMode()
    {
        reader.controls.UI.Disable();
        reader.controls.Player.Enable();
       
    }

    private void SetUIMode()
    {
        reader.controls.Player.Disable();
        reader.controls.UI.Enable();
       
    }

    private void DisableAll()
    {
        reader.controls.Disable();
 
    }
}