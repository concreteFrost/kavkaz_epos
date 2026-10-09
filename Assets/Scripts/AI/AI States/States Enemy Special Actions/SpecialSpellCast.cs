using System.Collections;
using UnityEngine;

public class SpecialSpellCast : BaseAISpecialMove
{
    HumanoidAIMotor motor;
    IEmitter emitter;
    CharacterSpellInventory spellInventory;
    public SpellProjectileSO specialSpell;

    private SpellProjectileSO lastSpell;

    Coroutine coroutine;

    HumanoidAICombatActions combatActions;

    float spawnStartTime = 0;
    [SerializeField] float minSpawnStartTime=0.3f;
    [SerializeField] float maxSpawnStartTime=0.7f;

    public override void Enter()
    {
       
        base.Enter();

        if (combatActions == null)
        {
            combatActions = new HumanoidAICombatActions(this);
        }

        spellInventory = context.spellInventory;
        emitter = context.emitter;
        lastSpell = spellInventory.CurrentItem.itemSO as SpellProjectileSO;

        spellInventory.CurrentItem.itemSO = specialSpell;
        motor = context.motor;

        coroutine = null;

        spawnStartTime =  Random.Range(minSpawnStartTime, maxSpawnStartTime);   


    }

    public override AIStateResult Run()
    {
        if (isFinished)
            return AIStateResult.Chase;

        if (coroutine == null)
        {
            
            coroutine = StartCoroutine(StartEmitWithDelay());
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

    IEnumerator StartEmitWithDelay()
    {
        motor.StopMovement();
        yield return new WaitForSeconds(spawnStartTime);
        combatActions.StartSpell(emitter, spellInventory, () => isFinished = true);
    }
}
