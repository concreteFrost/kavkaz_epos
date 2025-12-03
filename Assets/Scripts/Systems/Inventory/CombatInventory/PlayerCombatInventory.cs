using UnityEngine;

public class PlayerCombatInventory : CombatInventory
{
    ICharacterAnimator anim;

    public BareHandsWeapon bareHands;

    public void Init(ICharacterAnimator _anim)
    {
        
        anim = _anim;

        defaultWeapon = bareHands;

        defaultWeapon.Owner = this.tag; //заменить на id персонажа
        currentWeapon = defaultWeapon;

    }

    public override void SetWeapon(IWeapon w)
    {
       currentWeapon = w;
       anim.IsWeaponed = true; 
    }

    public override void SetShield(IWeapon w)
    {
        shieldWeapon = w;
    }

    public override void ResetWeapon()
    {

        //if (anim.isAttacking)
        //    return;

        anim.IsAttacking = false;   
        if (!currentWeapon.WeaponData().canOverride)
        {
            currentWeapon.ThrowWeapon();
        }
        
        currentWeapon = defaultWeapon;

        anim.IsWeaponed = false;    

    }

    public override void ResetShield()
    {

        if (shieldWeapon == null)
        {
            Debug.Log("shield is null");
            return;
        }

        shieldWeapon.ThrowWeapon();
        shieldWeapon = null;

    }


}
