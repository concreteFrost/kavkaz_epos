using System;
using UnityEngine;

public class HumanoidCombatInventory : BaseCombatInventory
{
    [Header("Bare Hands Settings")]
    [SerializeField] private MeleeData meleeData;

    public Action<ItemSO, IBreakable> WeaponDataUpdated;
    public Action<ItemSO, IBreakable> ShieldUpdated;

    public bool enableWeponBreakdown;


    public void Init(
        CharacterBoneSocket boneSocket,
        BaseHumanoidAnimatorController animatorController,
        IHumanoidMeleeCombat combatController,
        ICollector collector,
        bool enableWeaponBreakdown)
    {
        
        this.boneSocket = boneSocket;
        this.combatController = combatController;
        this.animatorController = animatorController;
        this.Collector = collector; 
        this.enableWeponBreakdown = enableWeaponBreakdown;


        DefaultWeapon = InitializeBarehands(collector);

        SetWeapon(GetStarterWeapon(collector,this.enableWeponBreakdown) ?? DefaultWeapon);
        ShieldWeapon = GetStarterShield(collector,this.enableWeponBreakdown) ?? null;

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
        Collector.Damagable.Protection = w;
        ShieldUpdated?.Invoke(ShieldWeapon.ShieldData(), ShieldWeapon);
    }

    public override void ResetCombatItem(CombatItem i)
    {
        switch (i)
        {
            case Weapon:
                ResetWeapon();
                break;
            case Shield:
                ResetShield();
                break;
            default: break;
        }
        
        
    }

    public void ResetWeapon()
    {
        CurrentWeapon = DefaultWeapon;
        combatController.IsWeaponed = false;
        WeaponDataUpdated?.Invoke(CurrentWeapon.WeaponData(), CurrentWeapon);

    }

    public void ResetShield()
    {

        if (ShieldWeapon == null) return;

        ShieldUpdated?.Invoke(ShieldWeapon.ShieldData(), null);
        Collector.Damagable.Protection = null;
        ShieldWeapon = null;
        combatController.IsShieldRaised = false;

       

    }


}

