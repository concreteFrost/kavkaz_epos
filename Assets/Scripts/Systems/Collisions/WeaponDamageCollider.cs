using UnityEngine;

public class WeaponDamageCollider : DamageCollider
{
    public IWeapon weaponOwner;

    public void SetWeapon(IWeapon weapon)
    {
        weaponOwner = weapon;
    }

    protected override void ApplyDamage(IDamagable target)
    {
        base.ApplyDamage(target);


        weaponOwner.ReduceDurability(
            weaponOwner.WeaponData().GetBreakdownPenalty()
        );
    }
}
