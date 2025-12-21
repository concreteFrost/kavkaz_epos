using System;
using UnityEngine;

public class HumanoidCombatControllerServices
{
    public IAttackSource combatInventory;
    public Animator animator;

    public HumanoidCombatControllerServices(IAttackSource combatInventory, Animator animator)
    {
        this.combatInventory = combatInventory;
        this.animator = animator;
    }
}
