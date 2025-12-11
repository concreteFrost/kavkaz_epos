using UnityEngine;

public class PlayerUIServiceLocator : MonoBehaviour
{
    [SerializeField] private PlayerStatsUI playerStatsUI;

    public PlayerStatsUI GetPlayerStatsUI() => playerStatsUI;
}
