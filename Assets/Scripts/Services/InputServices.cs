using System;

public class PlayerInputService
{
    public PlayerController controller;
    public PlayerAnimatorController animator;
    public PlayerTargetLock targetLock;

    public PlayerInputService(
        PlayerController controller,
        PlayerAnimatorController animator,
        PlayerTargetLock targetLock)
    {
        this.controller = controller;
        this.animator = animator;
        this.targetLock = targetLock;
    }
}



