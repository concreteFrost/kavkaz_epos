using System;
using UnityEngine;

public class HumanoidCombatInventory : BaseCombatInventory
{
    [Header("Bare Hands Settings")]
    [SerializeField] private MeleeData meleeData;

    public Action<ItemSO, IBreakable> WeaponDataUpdated;
    public Action<ItemSO, IBreakable> ShieldUpdated;

   
    public void Init(
        BaseHumanoidAnimatorController animatorController,
        IHumanoidMeleeCombat combatController,
        ICollector collector)
    {
 
        this.combatController = combatController;
        this.animatorController = animatorController;
        //InitializeBarehands();

        DefaultWeapon = InitializeBarehands(collector);

        SetWeapon(GetStarterWeapon(collector) ?? DefaultWeapon);
        ShieldWeapon = GetStarterShield(collector) ?? null;

    }
    // для UI обновления
    public void GetCurrentWeaponData()
    {
        WeaponDataUpdated?.Invoke(CurrentWeapon.WeaponData(), CurrentWeapon);
    }

    //для Ui обновления на старте
    public void GetCurrentShieldData()
    {
        if(ShieldWeapon == null)
        {
            ShieldUpdated?.Invoke(null, ShieldWeapon);  
            return;
        } 

        ShieldUpdated?.Invoke(ShieldWeapon.ShieldData(),ShieldWeapon);
    }

    private IWeapon InitializeBarehands(ICollector attackSource)
    {

        var bareHands = new MeleeWeapon();
        bareHands.Init(meleeData, attackSource);

        return bareHands;

    }

    public override void SetWeapon(IWeapon w)
    {
        CurrentWeapon = w;
        combatController.IsWeaponed = true;

        animatorController.OverrideArmed(w);

        WeaponDataUpdated?.Invoke(CurrentWeapon.WeaponData(), CurrentWeapon);

    }

    public override void SetShield(IShield w)
    {
        ShieldWeapon = w;

        ShieldUpdated?.Invoke(ShieldWeapon.ShieldData(), ShieldWeapon);
    }

    public override void ResetWeapon()
    {

        CurrentWeapon = DefaultWeapon;
        combatController.IsWeaponed = false;

        WeaponDataUpdated?.Invoke(CurrentWeapon.WeaponData(), CurrentWeapon);

    }

    public override void ResetShield()
    {

        if (ShieldWeapon == null) return;

        ShieldWeapon = null;
        combatController.IsShieldRaised = false;

        ShieldUpdated?.Invoke(ShieldWeapon.ShieldData(), ShieldWeapon);

    }


}

