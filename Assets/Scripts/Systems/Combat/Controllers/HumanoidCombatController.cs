using UnityEngine;

public class HumanoidCombatController : MonoBehaviour, ICharacterCombatAnimData
{

    //ссылки
    IAttackSource inventory;
    Animator animator;
    AnimatorOverrideController overrideController;

    // состояние
    internal bool isAttacking;
    internal bool isShieldRaised;
    internal bool isThrowingWeapon;
    internal int attackIndex = 0;
    internal bool isWeaponed;

    // буфер ввода для комбо
    internal float lastAttackInputTime = -10f;
    public float attackBufferTime = 0.35f; // время прожатия для продолжения комбо

    // очередь нажатий
    internal bool queuedAttack = false;

    // ================= свойства =================
 
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsWeaponed { get => isWeaponed; set => isWeaponed = value; }
    public bool IsShieldRaised { get => isShieldRaised; set => isShieldRaised = value; }
    public bool IsThrowingWeapon { get => isThrowingWeapon; set => isThrowingWeapon = value; }

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

        weapon.SelectAttack(attackIndex);

        var attack = weapon.CurrentAttack();

        var stateName = "Attack_" + attackIndex;
        overrideController[stateName] = attack.clip;

        animator.speed = attack.animationSpeed;
        animator.CrossFade(stateName, 0.15f, 2);

        isAttacking = true;
        attackIndex++;
    }

    public void EndAttack()
    {
        isAttacking = false;

        // проверяем буфер ввода
        if (Time.time - lastAttackInputTime <= attackBufferTime)
        {
            TryStartNextAttackFromQueue();
        }
        else
        {
            ResetCombo();
        }
    }

    internal void TryStartNextAttackFromQueue()
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
