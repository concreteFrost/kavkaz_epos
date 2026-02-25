using UnityEngine;
public class CharacterEmitter : Emitter
{
    CharacterSpellInventory spellInventory;
    BaseHumanoidAnimatorController animatorController;
    ITargetLocker targetLocker;

    public void Init(CharacterSpellInventory spellInventory,IAttackSource source, BaseHumanoidAnimatorController animatorController, ITargetLocker targetLocker, CharacterBoneSocket boneSockets)
    {
        this.spellInventory = spellInventory;   
        this.animatorController = animatorController;
        this.targetLocker = targetLocker;
        this.attackSource = source;
        this.emitSource = boneSockets.GetSpellCastSocket;

    }

    public override void StartEmit()
    {
        if(spellInventory.CurrentSpell == null)
        {
            Debug.Log("no spell available");
            return; 
        }

        var currentSpell = spellInventory.CurrentSpell;

        var spell = currentSpell.itemSO as SpellProjectileSO;
        projectileSO = spell;

        animatorController.OverrideSpell(spell);

        base.StartEmit();
        
        SetTargetData(targetLocker.CurrentTarget());
            
       

    }

    public override void Emit()
    {
        base.Emit();
        spellInventory.UseSpell();
    }
}
