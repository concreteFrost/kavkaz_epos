using System;
using System.Collections;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    ICharacterAnimator anim;

    PlayerCombatInventory inventory;    
    [SerializeField] private int totalClicks = 0;

    IEnumerator currentCoroutine = null;

    bool isInQueue = false;

    public void Init(ICharacterAnimator _anim, PlayerCombatInventory inv)
    {
        anim = _anim;
        inventory = inv;    
    }

    public void PerformAttack()
    {
        if (anim.IsJumping || !anim.IsGrounded)
            return;

        if (anim.IsShieldRaised)
        {
            ResetCombo();
            return;
        }

        // ставим атаку в очередь
        if (anim.IsAttacking)
        {
            isInQueue = true;
            return;
        }

        // начало цепочки атак
        totalClicks = 0;
        currentCoroutine = AttackCoroutine();
        StartCoroutine(currentCoroutine);
    }

    public void ThrowWeapon()
    {   
        inventory.CurrentWeapon.ThrowWeapon();
        inventory.ResetWeapon();    
    }


    public void PerformBlock()
    {
        if (inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.PerformDefence();
        anim.IsShieldRaised = true;

    }

    public void CancelBlock()
    {
        if(inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.CancelDefence();
        anim.IsShieldRaised = false;
    }

    internal void ThrowShield()
    {
        if (inventory.ShieldWeapon == null) return;

        inventory.ShieldWeapon.ThrowShield();
        inventory.ResetShield();
    }



    void ResetCombo()
    {
        totalClicks = 0;
        isInQueue = false;
       
        anim.IsAttacking = false;
        anim.AttackIndex = 0;   
    }

  
    IEnumerator AttackCoroutine()
    {

        var w = inventory.CurrentWeapon;

        var currentWeaponType = (int)w.WeaponData().attackSet.attackType;
        var currentAttakChain = w.WeaponData().attackSet;
        anim.WeaponIndex = currentWeaponType;

        while (true && !anim.IsShieldRaised)
        {
            anim.IsAttacking = true;
            anim.AttackIndex = totalClicks;

            float time = currentAttakChain.attackList[totalClicks].attackTime;

            yield return new WaitForSeconds(time);

            if (isInQueue)
            {
                isInQueue = false;

                totalClicks++;
                if (totalClicks > currentAttakChain.attackList.Count-1) // сбрасываем цепочку на начало
                    break;
            }
            else // если нет атак в очереди то сбрасываем на начало
            {
                
                break;
            }
        }

        ResetCombo();

        currentCoroutine = null;
    }


}
