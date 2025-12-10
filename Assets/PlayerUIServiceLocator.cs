using UnityEngine;

public class PlayerUIServiceLocator : MonoBehaviour
{
    [SerializeField] private PlayerStatsUI playerStatsUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        
    }

    public PlayerStatsUI GetPlayerStatsUI() => playerStatsUI;
}
