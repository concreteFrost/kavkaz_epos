using UnityEngine;

public class PlayerCombatActionHandler : MonoBehaviour
{
    PlayerActionGuards actionGuards;
    IHumanoidMeleeCombat combatController;
    IPushSource pushSource;
    IEmitter emitController;
    public void Init(PlayerActionGuards actionGuards, IHumanoidMeleeCombat combatController, IPushSource pushSource, IEmitter emitController)
    {
        this.actionGuards = actionGuards;
        this.combatController = combatController;   
        this.pushSource = pushSource;  
        this.emitController = emitController;   
    }

    #region Combat
    public void PerformAttack()
    {
        if (!actionGuards.CanAttack()) return;

        combatController.PerformAttack();

    }

    public void PerformPowerAttack()
    {
        if (!actionGuards.CanAttack()) return;

        combatController.PerformPowerAttack();
    }

    public void PerformBlock()
    {
        if (!actionGuards.CanBlock()) return;
        combatController.PerformBlock();
    }

    public void CancelBlock()
    {
        combatController.CancelBlock();
    }

    public void PerformPush()
    {
        if (!actionGuards.CanAttack()) return;

        pushSource.TriggerPushAnimation();
    }

    public void PerformEmit()
    {
        if (!actionGuards.CanEmit()) return;

        emitController.StartEmit();
    }

    #endregion
}
