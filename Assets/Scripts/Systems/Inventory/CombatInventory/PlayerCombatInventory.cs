using UnityEngine;

public class PlayerCombatInventory : CombatInventory
{
    ICharacterAnimator anim;

    public BareHandsWeapon bareHands;

    public override void Init(PlayerCombatInventoryServiceProvider service)
    {
        base.Init(service);

        anim = service.motor;
        Damagable = service.stats;

        bareHands.SetOwner(this);

        DefaultWeapon = bareHands;
        CurrentWeapon = DefaultWeapon;

    }

    public override void SetWeapon(IWeapon w)
    {
       CurrentWeapon = w;
       anim.IsWeaponed = true; 
     
    }

    public override void SetShield(IShield w)
    {
        ShieldWeapon = w;        
    }

    public override void ResetWeapon()
    {

        CurrentWeapon = DefaultWeapon;
        anim.IsAttacking = false;   
        anim.IsWeaponed = false;    

    }

    public override void ResetShield()
    {

        if (ShieldWeapon == null)
        {
            Debug.Log("shield is null");
            return;
        }

        ShieldWeapon = null;
        anim.IsShieldRaised = false;

    }


}
