using UnityEngine;

public class PlayerUIInput : MonoBehaviour
{
    private PlayerInputReader reader;
    [SerializeField] private PlayerUIManager manager;    


    public void Init(PlayerInputReader reader)
    {
        this.reader = reader;


        reader.controls.UI.HideAdditionalPanel.performed += _ => manager.HideContextMenu();
        reader.controls.UI.Slider.performed += c => manager.ReadSliderValue(c);
    }

    private void Update()
    {
        HandleCloseUI();
    }

    private void HandleCloseUI()
    {
        if (!reader.SwitchToGamePressed) return;

        GameStateManager.GameStateChanged?.Invoke(GameState.Game);
        reader.Consume(ref reader.SwitchToGamePressed);
    }



}