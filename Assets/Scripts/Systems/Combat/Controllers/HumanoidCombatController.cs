using System;
using UnityEngine;

public class HumanoidCombatController : MonoBehaviour, IHumanoidCombat
{

    //ссылки
    private IAttackSource inventory;
    Animator animator;
    AnimatorOverrideController overrideController;

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
        animator = service.animator;
       
        // создаём один общий OverrideController
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

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
        animator.CrossFade("Throw weapon", AnimatorParameters.transitionSpeed, AnimatorParameters.combatLayer);
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

        //назначаем атаку на плейсхолдер
        var stateName = "Attack_" + attackIndex;
        overrideController[stateName] = attack.animationInfo.clip;

        animator.speed = attack.animationInfo.animationSpeed;

        //делаем плавный переход на Attack_[index]
        animator.CrossFade(stateName, AnimatorParameters.transitionSpeed, 2);

        //movement.StopMove = true;
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
