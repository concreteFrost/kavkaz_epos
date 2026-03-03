using UnityEngine;
public class CharacterEmitter : Emitter
{
    CharacterSpellInventory spellInventory;
    CharacterStatsController statsController;
    BaseHumanoidAnimatorController animatorController;
    ITargetLocker targetLocker;

    public void Init(CharacterSpellInventory spellInventory, IAttackSource source, BaseHumanoidAnimatorController animatorController, ITargetLocker targetLocker, CharacterBoneSocket boneSockets, CharacterStatsController statsController)
    {
        this.spellInventory = spellInventory;
        this.animatorController = animatorController;
        this.targetLocker = targetLocker;
        this.attackSource = source;
        this.statsController = statsController;
        this.emitSource = boneSockets.GetSpellCastSocket;

    }

    public override void StartEmit()
    {
        if (spellInventory.CurrentItem == null)
        {
            Debug.Log("no spell available");
            return;
        }

        var currentSpell = spellInventory.CurrentItem;

        var spell = currentSpell.itemSO as SpellProjectileSO;
        var requiredModel = statsController.GetRequiredStatLevel(spell.Requirements.statType);

        if(requiredModel == 0)
        {
            Debug.Log("no rquired model found");
            return;
        }

        if (!spell.CanEmit(requiredModel)) return;

        animatorController.OverrideSpell(spell);

        projectileSO = spell;
        base.StartEmit();

        SetTargetData(targetLocker.CurrentTarget());
    }



    public override void Emit()
    {
        base.Emit();
        spellInventory.UseSpell();
    }
}