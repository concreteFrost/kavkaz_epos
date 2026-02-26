using UnityEngine;

public enum GameState
{
    Game=0,
    Inventory = 1

}
public class PlayerManager : MonoBehaviour
{
    public GameState State = GameState.Game;


   

    private void OnEnable()
    {
        //PlayerInput.PlayerModeChanged + = 
    }

    private void OnDisable()
    {
        
    }

}
