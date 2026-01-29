using UnityEngine;

public class WeaponDamageCollider : DamageCollider
{
    public IWeapon weaponData;

    public void SetWeaponData(IWeapon weapon)
    {
        weaponData = weapon;
    }

    public void SetDamageSource(Transform source)
    {
        this.attackSource = source;
    }

    protected override void ApplyDamage(IDamagable target)
    {

        base.ApplyDamage(target);

        weaponData.ReduceDurability(
            weaponData.WeaponData().GetBreakdownPenalty()
        );
    }
}