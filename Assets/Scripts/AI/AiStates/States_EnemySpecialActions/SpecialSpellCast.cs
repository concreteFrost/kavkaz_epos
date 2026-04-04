using System.Collections;
using UnityEngine;

public class SpecialSpellCast : BaseAISpecialMove
{

    IEmitter emitter;
    CharacterSpellInventory spellInventory;
    public SpellProjectileSO specialSpell;

    private SpellProjectileSO lastSpell;

    Coroutine coroutine;

    HumanoidAICombatActions combatActions;

    public override void Enter()
    {
        Debug.Log("entering spell cast");
        base.Enter();



        if (combatActions == null)
        {
            combatActions = new HumanoidAICombatActions(this);
        }

        spellInventory = context.spellInventory;
        emitter = context.emitter;
        lastSpell = spellInventory.CurrentItem.itemSO as SpellProjectileSO;

        spellInventory.CurrentItem.itemSO = specialSpell;

        coroutine = null;

       


    }

    public override AIStateResult Run()
    {
        if (isFinished)
            return AIStateResult.Chase;

        if (coroutine == null)
        {
            Debug.Log("starting summon coroutine");
            coroutine = combatActions.StartSpell(emitter, spellInventory, () => isFinished = true);
        }
          

        return AIStateResult.None;
    }


    public override void Exit()
    {
        spellInventory.CurrentItem.itemSO = lastSpell;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        base.Exit();

       

    }
}
