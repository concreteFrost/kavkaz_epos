using UnityEngine;

public class CharacterEmitter : Emitter
{

    BaseHumanoidAnimatorController animatorController;
    ITargetLocker targetLocker;


    public void Init(IAttackSource source, BaseHumanoidAnimatorController animatorController, ITargetLocker targetLocker)
    {
        this.animatorController = animatorController;
        this.targetLocker = targetLocker;
        this.attackSource = source;

    }

    public override void StartEmit()
    {
        base.StartEmit();
        SetTargetData(targetLocker.CurrentTarget());
            
        var spell = projectileSO as SpellProjectileSO;
        animatorController.OverrideSpell(spell);

    }
}
