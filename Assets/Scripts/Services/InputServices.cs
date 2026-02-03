using System;

public class PlayerInputService
{
    public PlayerController controller;
    public PlayerAnimatorController animator;
    public PlayerTargetLock targetLock;
    public AgressivePushController pushController;

    public PlayerInputService(
        PlayerController controller,
        PlayerAnimatorController animator,
        PlayerTargetLock targetLock,
        AgressivePushController pushController)
    {
        this.controller = controller;
        this.animator = animator;
        this.targetLock = targetLock;
        this.pushController = pushController;
    }
}



