using UnityEngine;

public class PlayerTargetLockService
{
    public LockOnTargetUI lockOnTargetUI;
    public PlayerController controller;
    public IDamagable damageController;
    public CharacterStatsController statsManager;

    public PlayerTargetLockService(
        LockOnTargetUI lockOnTargetUI,
        PlayerController controller,
        IDamagable damageController,
        CharacterStatsController stats
        )
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.damageController = damageController;
        this.statsManager = stats;
    }
}


