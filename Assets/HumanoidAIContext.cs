using UnityEngine;

public class HumanoidAIContext
{
    public Transform self;

    public HumanoidAIMotor motor;
    public HumanoidAIController controller;
    public HumanoidCombatController combat;
    public HumanoidCombatInventory inventory;
    public CharacterFOV fov;
    public CharacterInteract interact;

    public Transform currentTarget;

    public HumanoidAIContext(Transform self, HumanoidAIMotor motor, HumanoidAIController controller, HumanoidCombatController combat, HumanoidCombatInventory inventory, CharacterFOV fov, CharacterInteract interact, Transform currentTarget)
    {
        this.self = self;
        this.motor = motor;
        this.controller = controller;
        this.combat = combat;
        this.inventory = inventory;
        this.fov = fov;
        this.interact = interact;
        this.currentTarget = currentTarget;
    }
}
