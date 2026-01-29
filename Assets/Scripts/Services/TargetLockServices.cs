using UnityEngine;

public class PlayerTargetLockService
{
    public LockOnTargetUI lockOnTargetUI;
    public PlayerController controller;
    public IDamagable damageController;
    public HumanoidStats stats;

    public PlayerTargetLockService(
        LockOnTargetUI lockOnTargetUI,
        PlayerController controller,
        IDamagable damageController,
        HumanoidStats stats
        )
    {
        this.lockOnTargetUI = lockOnTargetUI;
        this.controller = controller;
        this.damageController = damageController;
        this.stats = stats;
    }
}


