using UnityEngine;

public class PlayerTargetLockService
{
    public LockOnTargetUI lockOnTargetUI;
    public PlayerController controller;
    public IDamagable damageController;
    public HumanoidStatsManager statsManager;

    public PlayerTargetLockService(
        LockOnTargetUI lockOnTargetUI,
        PlayerController controller,
        IDamagable damageController,
        HumanoidStatsManager stats
        )
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.damageController = damageController;
        this.statsManager = stats;
    }
}


