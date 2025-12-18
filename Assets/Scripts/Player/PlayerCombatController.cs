using System.Collections;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour , ICharacterCombatAnimData
{
    PlayerCombatInventory inventory;
   
    [SerializeField] private int totalClicks = 0;

    IEnumerator currentCoroutine = null;

    bool isInQueue = false;

    internal bool isAttacking;
    internal int attackIndex = 0;
    internal int weaponIndex = 0;
    internal bool isShieldRaised;
    internal bool isWeaponed;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsWeaponed { get => isWeaponed; }
    public int AttackIndex { get => attackIndex; }
    public int WeaponIndex { get => weaponIndex; }
    public bool IsShieldRaised { get => isShieldRaised; }

    public void Init(PlayerCombatControllerServiceProvider service)
    {
        inventory = service.combatInventory; 
    }

    public void PerformAttack()
    {

        if (isShieldRaised)
        {
            ResetCombo();
            return;
        }

        // ставим атаку в очередь
        if (isAttacking)
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
        isShieldRaised = true;

    }

    public void CancelBlock()
    {
        if(inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.CancelDefence();
        isShieldRaised = false;
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
       
        isAttacking = false;
        attackIndex = 0;   
    }

    IEnumerator AttackCoroutine()
    {

        var w = inventory.CurrentWeapon;

        var currentWeaponType = (int)w.WeaponData().attackSet.attackType;
        var currentAttakChain = w.WeaponData().attackSet;
        weaponIndex = currentWeaponType;

        while (true && !isShieldRaised)
        {
            isAttacking = true;
            attackIndex = totalClicks;

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
