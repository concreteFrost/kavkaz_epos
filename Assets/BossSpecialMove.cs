using System.Collections;
using UnityEngine;
public class BossSpecialMove : AIState<EnemyBrainContext>
{
    IEmitter emitter;
    CharacterSpellInventory spellInventory;
    public SpellProjectileSO specialSpell;

    private SpellProjectileSO lastSpell;

    EnemyBrain brain;

    Coroutine coroutine;

    public void Init(EnemyBrain brain)
    {
         this.brain = brain;    
    }
    public override void Enter()
    {
        spellInventory = context.spellInventory;
        emitter = context.emitter;  
        lastSpell = spellInventory.CurrentItem.itemSO as SpellProjectileSO;

        spellInventory.CurrentItem.itemSO = specialSpell;
    }

    public override void Exit()
    {

        spellInventory.CurrentItem.itemSO = lastSpell;

        if(coroutine != null)
        {
            StopCoroutine(coroutine);   
            coroutine = null;   
        }
       
    }

    public override AIStateResult Run()
    {
        if (coroutine != null) return AIStateResult.None;

        coroutine = StartCoroutine(AttackCoroutine());

        return AIStateResult.Chase;
    }

    IEnumerator AttackCoroutine()
    {
        //пополняем запасы чтобы магия не истощалась 
        spellInventory.TopUpCurrentItem(1);

        emitter.StartEmit();
        while (emitter.IsEmitting)
            yield return null;

        //brain.ForceChase();
    }

}