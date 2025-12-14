using UnityEngine;

public class PlayerUIServiceLocator : MonoBehaviour
{
    [SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private LockOnTargetUI lockOnTargetUI; 
    public PlayerStatsUI GetPlayerStatsUI() => playerStatsUI;
    public LockOnTargetUI GetLockOnTargetUI() => lockOnTargetUI;
}
