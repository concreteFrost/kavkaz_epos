using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CombatItem, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    private WeaponAttack currentAttack;

    [SerializeField] private WeaponDamageCollider damageCollider;

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

    public override void Init(ItemData data)
    {
        base.Init(data);

        damageCollider.Init();
        damageCollider.SetWeaponData(this);

    }

    public void PerformAttack()
    {
        if (currentAttack == null || Owner == null) return;

        var baseWeaponDamage = WeaponData().GetBaseDamage();

        if (data.durability <= 0) 
            baseWeaponDamage = baseWeaponDamage * 0.5f;

        var ownerStrengthMultiplier = Owner.StatsController.Strength.CurrentMax;

        DamageData damageData = currentAttack.damageData;
        damageData.SetFinalDamage(baseWeaponDamage,ownerStrengthMultiplier);

        damageCollider.EnableCollider(
            damageData,
            Owner.AttackSource.TargetsToIgnore,
            Owner.AttackSource
        );
    }

    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }

    public override void AssignToOwner(IInteractor target)
    {
        Owner = target;

        damageCollider.SetWeaponData(this);

        AssignParent(Owner.CombatInventory.GetRightHand());
    }


   
}