using UnityEngine;

public class PlayerUIInput : MonoBehaviour
{
    private PlayerInputReader reader;
    [SerializeField] private PlayerUIManager manager;    


    public void Init(PlayerInputReader reader)
    {
        this.reader = reader;
    }

    private void Update()
    {
        HandleCloseUI();

        if(reader.SliderScroll != 0)
        {
            manager.ReadSliderValue(reader.SliderScroll);
            reader.ConsumeScroll(ref reader.SliderScroll);
        }

        if (reader.HideContextPressed)
        {
            manager.HideContextMenu();
            reader.Consume(ref reader.HideContextPressed);
        }
    }

    private void HandleCloseUI()
    {
        if (!reader.SwitchToGamePressed) return;

        reader.Consume(ref reader.SwitchToGamePressed);
        GameStateManager.GameStateChanged?.Invoke(GameState.Game);
       
    }



}