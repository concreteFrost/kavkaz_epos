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

        SetGameMode(); // по умолчанию
    }


    private void OnDisable()
    {
        GameStateManager.GameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
       
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
            case GameState.Bonfire:
                SetUIMode();
                break;
            case GameState.Dialogue:
                SetDialogueMode();
                break;
            case GameState.Transition:
                DisableAll();
                break;

        }
    }

    private void SetGameMode()
    {
        reader.controls.Player.Enable();
        reader.controls.UI.Disable();
        reader.controls.Dialogue.Disable();
       
    }

    private void SetUIMode()
    {
        reader.controls.Player.Disable();
        reader.controls.UI.Enable();
        reader.controls.Dialogue.Disable();
       
    }

    private void SetDialogueMode()
    {
        
        reader.controls.Dialogue.Enable();
        reader.controls.Player.Disable();
        reader.controls.UI.Disable();
    }

    private void DisableAll()
    {
        reader.controls.Disable();
    }
}