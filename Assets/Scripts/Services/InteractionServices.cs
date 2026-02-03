
using UnityEngine;

public class HumanoidInteractService
{
    public Transform self;
    public BaseHumanoidAnimatorController animatorController;
    public ICombatInventory combatInventory;
    public IDamagable owner;
    public IAttackSource attackSource;

    public HumanoidInteractService(Transform self, BaseHumanoidAnimatorController animatorController, ICombatInventory combatInventory, IDamagable owner, IAttackSource attackSource)
    {
        this.self = self;   
        this.animatorController = animatorController;
        this.combatInventory = combatInventory;
        this.owner = owner;
        this.attackSource = attackSource;
    }
}
