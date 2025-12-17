using System.Collections;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    PlayerMotor motor;
    PlayerCombatInventory inventory;  
    PlayerStats stats;
    [SerializeField] private int totalClicks = 0;

    IEnumerator currentCoroutine = null;

    private float currentAttackTimer = 0f;

    bool isInQueue = false;

    public void Init(PlayerCombatControllerServiceProvider service)
    {
        motor = service.motor;
        inventory = service.combatInventory; 
        stats = service.stats;
    }

    public void PerformAttack()
    {
        if (motor.IsJumping || !motor.IsGrounded || stats.currentStamina <=0)
            return;

        if (motor.IsShieldRaised)
        {
            ResetCombo();
            return;
        }

        // ставим атаку в очередь
        if (motor.IsAttacking)
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
        //inventory.ResetWeapon();    
    }


    public void PerformBlock()
    {
        if (inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.PerformDefence();
        motor.isShieldRaised = true;

    }

    public void CancelBlock()
    {
        if(inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.CancelDefence();
        motor.isShieldRaised = false;
    }

    public void ThrowShield()
    {
        if (inventory.ShieldWeapon == null) return;

        inventory.ShieldWeapon.ThrowShield();
        //inventory.ResetShield();
    }

    void ResetCombo()
    {
        totalClicks = 0;
        isInQueue = false;
       
        motor.isAttacking = false;
        motor.attackIndex = 0;   
    }

  
    public void SetCurrentAttackTimer(float timer)
    {
        currentAttackTimer = timer; 
    }
    IEnumerator AttackCoroutine()
    {

        var w = inventory.CurrentWeapon;

        var currentWeaponType = (int)w.WeaponData().attackSet.attackType;
        var currentAttakChain = w.WeaponData().attackSet;
        motor.weaponIndex = currentWeaponType;

        while (true && !motor.IsShieldRaised)
        {
            motor.isAttacking = true;
            motor.attackIndex = totalClicks;

            var currentAttack = currentAttakChain.attackList[totalClicks];

            w.SetCurrentAttack(currentAttack);
            float time = currentAttack.attackTime;

            yield return new WaitForSeconds(time);

            if (isInQueue)
            {
                   
                totalClicks++;

                isInQueue = false;
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
