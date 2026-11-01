using UnityEngine;
using System;

public class BaseHumanoidCombatController : MonoBehaviour, IHumanoidMeleeCombat
{
    //ссылки
    public IWeaponSetter inventory;
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
    private bool loopCurrentCombo;
    private bool hasStartedCombo;
    private Func<WeaponAttack, bool> comboAttackCheck;

    // ================= свойства =================
    public float AttackBufferTime { get => attackBufferTime; set=>attackBufferTime = value; }   

    public bool IsAttacking { get; set; }
    public bool ShouldPlayAttackAudio { get; private set; } = true;
    public bool IsWeaponed { get; set; }
    public bool IsShieldRaised { get; set; }

    public void Init(
        IWeaponSetter combatInventory,
        BaseHumanoidAnimatorController animatorController,
        IDamagable damageController)
    {
        this.inventory = combatInventory;
        this.animatorController = animatorController;
        this.damageController = damageController;

        ResetCombo();
    }

    public void ForceAttackCancel()
    {
        inventory.CurrentWeapon?.CancelAttack();
        ResetCombo();
    }


    public void PerformAttack(bool loopCombo = false, Func<WeaponAttack, bool> canStartComboAttack = null)
    {
        if (!CanStartAttack()) return;

        lastAttackInputTime = Time.time;
        loopCurrentCombo = loopCombo;
        comboAttackCheck = canStartComboAttack;

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
        if (!CanStartAttack()) return;

        if (IsAttacking) return;

        ResetCombo();

        var weapon = inventory.CurrentWeapon;
        var powerAttack = weapon.WeaponData().attackSet.powerAttack;

        weapon.GetPowerAttack(powerAttack);

        animatorController.OverrideAttack(powerAttack, "Power_Attack");

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


    // ================= КОМБО =================
    internal void StartNextAttack()
    {
        if (!CanStartAttack())
        {
            ForceAttackCancel();
            return;
        }

        var weapon = inventory.CurrentWeapon;
        var attackSet = weapon.WeaponData().attackSet;

        if (attackSet == null || attackSet.attackList == null || attackSet.attackList.Count == 0)
        {
            Debug.Log("no attacks assigned for this weapon");
            return;
        }

        if (attackIndex >= attackSet.attackList.Count)
        {
            if (!loopCurrentCombo)
            {
                ResetCombo();
                return;
            }

            // ИИ может повторять набор, пока исполнитель серии не исчерпал число ударов.
            attackIndex = 0;
        }

        //выбираем атаку из списка
        // РџСЂРѕРІРµСЂСЏРµРј СЃР»РµРґСѓСЋС‰РёР№ СѓРґР°СЂ РїСЂРё Р·Р°РїСѓСЃРєРµ: С†РµР»СЊ РјРѕРіР»Р° РѕС‚РѕР№С‚Рё РїРѕСЃР»Рµ РїРѕСЃС‚Р°РЅРѕРІРєРё РІ РѕС‡РµСЂРµРґСЊ.
        if (comboAttackCheck != null && !comboAttackCheck(attackSet.attackList[attackIndex]))
        {
            ResetCombo();
            return;
        }

        // Р—Р°С†РёРєР»РµРЅРЅР°СЏ СЃРµСЂРёСЏ РР Р·РІСѓС‡РёС‚ С‚РѕР»СЊРєРѕ РЅР° РїРµСЂРІРѕРј СѓРґР°СЂРµ, РІРєР»СЋС‡Р°СЏ РїРѕРІС‚РѕСЂ СЃРїРёСЃРєР° Р°С‚Р°Рє.
        ShouldPlayAttackAudio = !loopCurrentCombo || !hasStartedCombo;
        hasStartedCombo = true;
        weapon.SelectAttack(attackIndex);

        var attack = weapon.CurrentAttack();

        string attackName = "Attack_" + attackIndex;

        animatorController.OverrideAttack(attack, attackName);

        IsAttacking = true;
        attackIndex++;
    }

    public void EndAttack()
    {
        // Выход из анимации после отмены не должен возобновлять очередь комбо.
        if (!IsAttacking)
            return;

        if (!CanStartAttack())
        {
            ForceAttackCancel();
            return;
        }

        IsAttacking = false;
        OnAttackEnd?.Invoke();
        //movement.StopMove = false;  

        // проверяем буфер ввода
        if (queuedAttack && Time.time - lastAttackInputTime <= attackBufferTime)
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

    private bool CanStartAttack() =>
        !IsShieldRaised && !damageController.IsDamaged &&
        !damageController.IsKnockedOut && !damageController.IsDead;

    public void ResetCombo()
    {
        attackIndex = 0;
        IsAttacking = false;
        queuedAttack = false;
        loopCurrentCombo = false;
        hasStartedCombo = false;
        ShouldPlayAttackAudio = true;
        comboAttackCheck = null;

    }

}
