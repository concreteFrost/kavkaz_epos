using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CombatItem, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    private WeaponAttack currentAttack;

    [SerializeField] private WeaponDamageCollider damageCollider;

    public ICollector Owner { get; set; }

    private float minStopVelocity = 0.2f;

    int currentAttackIndex = 0;

    #region IWeapon Contract
    public WeaponSO WeaponData() => weaponSO;
    public WeaponAttack CurrentAttack() => currentAttack;
    public WeaponAttack GetPowerAttack(WeaponAttack attack) => currentAttack = attack;
    public void SelectAttack(int index)
    {
        var list = weaponSO.attackSet.attackList;

        if (index < 0 || index >= list.Count)
        {
            currentAttackIndex = 0;
        }
        else
        {
            currentAttackIndex = index;
        }

        currentAttack = list[currentAttackIndex];
    }
    #endregion

    public override void Init(ItemSO itemData)
    {
        
        base.Init(itemData);

        ToggleInteraction(true);

        damageCollider.SetWeaponData(this);

    }

    public void PerformAttack()
    {
        if (currentAttack == null) return;

        DamageData damageData = new DamageData()
        {
            healthDamageMultiplier = currentAttack.GetFinalHealthDamage(weaponSO.GetBaseDamage()),
            balanceDamageType = currentAttack.damageData.balanceDamageType,
            impactForce = currentAttack.damageData.impactForce
        };

        damageCollider.EnableCollider(
            damageData,
            Owner.AttackSource.TargetsToIgnore
        );
    }

    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }
    public override void PickUp(ICollector collector)
    {
        if (!collector.CombatInventory.CanPickWeapon()) return;

        if (breakdownThreshold <= 0)
        {
            Debug.Log("this weapon is broken");
            return;
        }

        AssignToOwner(collector);

    }

    public void AssignToOwner(ICollector target)
    {
        Owner = target;

        damageCollider.SetWeaponData(this);
        damageCollider.SetDamageSource(Owner.AttackSource.Source());

        AssignParent(Owner.CombatInventory.GetRightHand());
        ToggleInteraction(false);


        target.CombatInventory.SetWeapon(this);
    }

    public void ReduceDurability(float amount)
    {
        breakdownThreshold -= amount;

        if (breakdownThreshold <= 0)
        {
            Owner.CombatInventory.ResetWeapon();
            DropWeapon();
        }
    }

    public void DropWeapon()
    {
        ResetParent();
        ResetOwner();
        ToggleInteraction(true);


    }

    public void ThrowWeapon(Transform from, float force)
    {
        var tempTargets = Owner.AttackSource.TargetsToIgnore;
        var source = Owner.AttackSource.Source();

        ResetParent();
        ToggleInteraction(true);
       

        rb.AddForce(from.forward * force, ForceMode.Impulse);

        StartCoroutine(ThrowCoroutine(tempTargets, source));
        StartCoroutine(DisableColliderWhenStopped());

     
    }

    private void ResetOwner()
    {
        Owner.CombatInventory.ResetWeapon();
        Owner = null;
        damageCollider.SetDamageSource(null);
    }

    IEnumerator ThrowCoroutine(List<CharacterType> targetsToIgnore, Transform source)
    {
        DamageData damageData = new DamageData()
        {
            healthDamageMultiplier = weaponSO.GetBaseDamage(),
            balanceDamageType = BalanceDamageType.High,
            impactForce = 10f
        };

        damageCollider.SetDamageSource(source);
        damageCollider.EnableCollider(damageData, targetsToIgnore);

        yield return null;

        
    }

    IEnumerator DisableColliderWhenStopped()
    {

        yield return new WaitUntil(() => rb.linearVelocity.sqrMagnitude > 0.15f);


        while (true)
        {
            if (rb.linearVelocity.magnitude < minStopVelocity)
            {
                damageCollider.DisableCollider();
                ResetOwner();
                yield break;
            }

            yield return null;
        }

    }


}