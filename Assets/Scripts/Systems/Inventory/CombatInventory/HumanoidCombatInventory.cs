using System.Diagnostics;
using UnityEngine;

public class HumanoidCombatInventory : BaseCombatInventory
{
    [Header("Bare Hands Settings")]

    [SerializeField] private WeaponSO barehandsData;
    [SerializeField] private WeaponDamageCollider barehandDamageCollider;
    

    public override void Init(HumanoidCombatInventoryService service)
    {
        base.Init(service);

        InitializeBarehands();

        DefaultWeapon = InitializeBarehands();

        CurrentWeapon = GetStarterWeapon(service.collector) ?? DefaultWeapon;
        ShieldWeapon = GetStarterShield(service.collector) ?? null;
        
    }

    

    private IWeapon InitializeBarehands()
    {

        var bareHands = new BareHandsWeapon();
        bareHands.Init(barehandsData, barehandDamageCollider, this);

        return bareHands;

    }

    public override void SetWeapon(IWeapon w)
    {
       CurrentWeapon = w;
       combatController.IsWeaponed = true; 
     
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
