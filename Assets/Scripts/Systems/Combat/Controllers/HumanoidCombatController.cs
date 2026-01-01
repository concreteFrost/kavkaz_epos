using UnityEngine;

public class HumanoidCombatController : MonoBehaviour, ICharacterCombatAnimData
{
    IAttackSource inventory;

    // состояние
    internal bool isAttacking;
    internal bool attackFired = false;
    internal bool isShieldRaised;
    internal bool isThrowingWeapon;
    public int attackIndex = 0;
    public int weaponIndex = 0;
    internal bool isWeaponed;

    // буфер ввода для комбо
    internal float lastAttackInputTime = -10f;
    public float attackBufferTime = 0.35f; // время, чтобы продолжить комбо

    // ================= свойства =================
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsWeaponed { get => isWeaponed; set => isWeaponed = value; }
    public int AttackIndex { get => attackIndex; }
    public int WeaponIndex { get => weaponIndex; }
    public bool IsShieldRaised { get => isShieldRaised; set => isShieldRaised = value; }
    public bool IsThrowingWeapon { get => isThrowingWeapon; set => isThrowingWeapon = value; }

    // ================= INIT =================
    public void Init(HumanoidCombatControllerServices service)
    {
        inventory = service.combatInventory;
        ResetCombo();
    }

    // ================= INPUT =================
    public void PerformAttack()
    {
        if (isShieldRaised) return;
       
        lastAttackInputTime = Time.time;

        weaponIndex = (int)inventory.CurrentWeapon.WeaponData().weaponType;
        isAttacking = true;
    }

    public void PerformBlock()
    {
        if (inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.PerformDefence();
        isShieldRaised = true;
    }

    public void CancelBlock()
    {
        if (inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.CancelDefence();
        isShieldRaised = false;
    }

    public void ThrowWeapon()
    {
        ResetCombo();
        isThrowingWeapon = true;
    }

    public void ThrowShield()
    {
        if (inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.ThrowShield();
    }

    // ================= комбо =================
    public void StartAttack()
    {

        var w = inventory.CurrentWeapon;
        var attackSet = w.WeaponData().attackSet;

        if (attackIndex >= attackSet.attackList.Count-1)
        {
            ResetCombo();
            return;
        }

        isAttacking = true;
        var attack = attackSet.attackList[attackIndex];
        w.SetCurrentAttack(attack);
       

        attackIndex++;
    }

    public void EndAttack()
    {
        isAttacking = false;
        attackFired = false;

        // проверка буфера ввода
        if (Time.time - lastAttackInputTime > attackBufferTime)
        {
            ResetCombo();
        }
    }

    void ResetCombo()
    {
        attackIndex = 0;
        isAttacking = false;
    }
}
