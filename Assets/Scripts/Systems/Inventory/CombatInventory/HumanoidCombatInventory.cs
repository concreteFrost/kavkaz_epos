using UnityEngine;

public class HumanoidCombatInventory : BaseCombatInventory
{
    [Header("Bare Hands Settings")]
    [SerializeField] private MeleeData meleeData;

    public void Init(
        BaseHumanoidAnimatorController animatorController,
        IHumanoidCombat combatController,
        ICollector collector)
    {

        this.combatController = combatController;
        this.animatorController = animatorController;
        //InitializeBarehands();


        DefaultWeapon = InitializeBarehands(collector.AttackSource);

        SetWeapon(GetStarterWeapon(collector) ?? DefaultWeapon);
        ShieldWeapon = GetStarterShield(collector) ?? null;

    }


    private IWeapon InitializeBarehands(IAttackSource attackSource)
    {

        var bareHands = new MeleeWeapon();
        bareHands.Init(meleeData, this, attackSource);

        return bareHands;

    }

    public override void SetWeapon(IWeapon w)
    {
        CurrentWeapon = w;
        combatController.IsWeaponed = true;

        animatorController.OverrideArmed(w);

    }

    public override void SetShield(IShield w)
    {
        ShieldWeapon = w;
    }

    public override void ResetWeapon()
    {

        CurrentWeapon = DefaultWeapon;
        combatController.IsWeaponed = false;

    }

    public override void ResetShield()
    {

        if (ShieldWeapon == null) return;

        ShieldWeapon = null;
        combatController.IsShieldRaised = false;

    }


}
