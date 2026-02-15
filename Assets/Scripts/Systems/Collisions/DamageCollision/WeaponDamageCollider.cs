using UnityEngine;

public class WeaponDamageCollider : DamageCollider
{
    public IWeapon weaponData;

    public void SetWeaponData(IWeapon weapon)
    {
        weaponData = weapon;
    }

    protected override void ApplyDamage(IDamagable target)
    {
        weaponData.ReduceDurability(
           weaponData.WeaponData().GetBreakdownPenalty()
       );

        base.ApplyDamage(target);

       
    }
}