using UnityEngine;

public class WeaponDamageCollider : DamageCollider
{
    public IWeapon weaponData;

    public void SetWeapon(IWeapon weapon, Transform source)
    {
        weaponData = weapon;
        this.source = source;  
        
    }

    protected override void ApplyDamage(IDamagable target)
    {
       
        base.ApplyDamage(target);

        weaponData.ReduceDurability(
            weaponData.WeaponData().GetBreakdownPenalty()
        );
    }
}
