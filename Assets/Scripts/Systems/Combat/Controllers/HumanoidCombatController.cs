using System;
using UnityEngine;

public class HumanoidCombatController : MonoBehaviour, IHumanoidCombat
{

    //ссылки
    private ICombatInventory inventory;
    private BaseHumanoidAnimatorController animatorController;

    public event Action OnAttackEnd; // для ИИ чтобы знать когда закончилась атака и начать новую

    // состояние
    internal bool isAttacking;
    internal bool isShieldRaised;
    internal int attackIndex = 0;
    internal bool isWeaponed;

    // буфер ввода для комбо
    internal float lastAttackInputTime = -10f;
    public float attackBufferTime = 0.35f; // время прожатия для продолжения комбо

    // очередь нажатий
    internal bool queuedAttack = false;

    // ================= свойства =================
 
    public bool IsWeaponed { get => isWeaponed; set => isWeaponed = value; }
    public bool IsShieldRaised { get => isShieldRaised; set => isShieldRaised = value; }


    // ================= INIT =================
    public void Init(HumanoidCombatControllerServices service)
    {
        inventory = service.combatInventory;
        this.animatorController = service.animatorController;

        ResetCombo();
    }

    // ================= INPUT =================
    public void PerformAttack()
    {
        if (isShieldRaised) return;

        lastAttackInputTime = Time.time;
       
        // если уже атакуем — ставим в очередь
        if (isAttacking)
        {
            queuedAttack = true;
            return;
        }

        // иначе запускаем атаку сразу
        StartNextAttack();
    }

    public void PerformBlock()
    {
        if (inventory.ShieldWeapon == null) return;
        inventory.ShieldWeapon.PerformDefence();
        isShieldRaised = true;
    }

    public void CancelBlock()
    {
        isShieldRaised = false;

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

        if (attackIndex >= attackSet.attackList.Count)
        {
            ResetCombo();
            return;
        }

        //выбираем атаку из списка
        weapon.SelectAttack(attackIndex);

        var attack = weapon.CurrentAttack();

        animatorController.OverrideAttack(attack, attackIndex);
        
        isAttacking = true;
        attackIndex++;
    }

    public void EndAttack()
    {
        isAttacking = false;
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

    void ResetCombo()
    {
        attackIndex = 0;
        isAttacking = false;
        queuedAttack = false;
        
    }
}
