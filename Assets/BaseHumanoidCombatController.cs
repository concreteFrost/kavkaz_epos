using UnityEngine;
using System;

public class BaseHumanoidCombatController<T> : MonoBehaviour, IHumanoidCombat where T : BaseHumanoidCombatControllerServices
{
    //ссылки
    public ICombatInventory inventory;
    protected IDamagable damageController;
    protected BaseHumanoidAnimatorController animatorController;

    public event Action OnAttackEnd; // для ИИ чтобы знать когда закончилась атака и начать новую

    // состояние
    internal int attackIndex = 0;

    // буфер ввода для комбо
    internal float lastAttackInputTime = -10f;
    public float attackBufferTime = 0.35f; // время прожатия для продолжения комбо

    // очередь нажатий
    internal bool queuedAttack = false;

    // ================= свойства =================

    public bool IsAttacking { get; set; }
    public bool IsWeaponed { get; set; }
    public bool IsShieldRaised { get; set; }

    public virtual void Init(T service)
    {
        inventory = service.combatInventory;
        this.animatorController = service.animatorController;
        this.damageController = service.damageController;
        //this.pushReceiver = service.pushable;


        ResetCombo();

        damageController.DamageTaken += ForceAttackCancel;
    }

    protected void ForceAttackCancel(Transform source)
    {
        
        inventory.CurrentWeapon.CancelAttack();
        ResetCombo();
    }

    protected virtual void OnDisable()
    {
        damageController.DamageTaken -= ForceAttackCancel;
    }


    public void PerformAttack()
    {
        if (IsShieldRaised) return;

        lastAttackInputTime = Time.time;

        // если уже атакуем — ставим в очередь
        if (IsAttacking)
        {
            queuedAttack = true;
            return;
        }

        // иначе запускаем атаку сразу
        StartNextAttack();
    }

    public void PerformPowerAttack()
    {
        if (IsShieldRaised) return;

        if (IsAttacking) return;

        ResetCombo();

        var weapon = inventory.CurrentWeapon;
        var powerAttack = weapon.WeaponData().attackSet.powerAttack;

        weapon.GetPowerAttack(powerAttack);

        animatorController.OverrideAttack(powerAttack, "Power Attack");

        IsAttacking = true;
    }

    public void PerformBlock()
    {
        if (inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.PerformDefence();
        IsShieldRaised = true;
    }

    public void CancelBlock()
    {
        IsShieldRaised = false;

        if (inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.CancelDefence();

    }

    public void ThrowWeapon()
    {
        ResetCombo();
        //isThrowingWeapon = true;
        animatorController.PerformThrow();
    }

    public void ThrowShield()
    {
        if (inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.ThrowShield();
    }

    // ================= КОМБО =================
    internal void StartNextAttack()
    {
        var weapon = inventory.CurrentWeapon;
        var attackSet = weapon.WeaponData().attackSet;

        if (attackSet == null)
        {
            Debug.Log("no attacks assigned for this weapon");
            return;
        }

        if (attackIndex >= attackSet.attackList.Count)
        {
            ResetCombo();
            return;
        }

        //выбираем атаку из списка
        weapon.SelectAttack(attackIndex);

        var attack = weapon.CurrentAttack();

        string attackName = "Attack_" + attackIndex;

        animatorController.OverrideAttack(attack, attackName);

        IsAttacking = true;
        attackIndex++;
    }

    public void EndAttack()
    {
        IsAttacking = false;
        OnAttackEnd?.Invoke();
        //movement.StopMove = false;  

        // проверяем буфер ввода
        if (Time.time - lastAttackInputTime <= attackBufferTime)
        {
            TryStartNextAttackFromQueue();
        }
        else
        {
            ResetCombo();
        }

        //TryStartNextAttackFromQueue();
    }

    public void TryStartNextAttackFromQueue()
    {
        if (queuedAttack)
        {
            queuedAttack = false;
            StartNextAttack();
        }
    }

    public void ResetCombo()
    {
        attackIndex = 0;
        IsAttacking = false;
        queuedAttack = false;

    }

}
