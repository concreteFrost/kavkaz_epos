using UnityEngine;

public class WeaponDamageCollider : DamageCollider
{
    public IWeapon weaponOwner; // Ссылка на оружие, состояние которого будет уменьшаться

    public void SetWeapon(IWeapon _weapon)
    {
        weaponOwner = _weapon;
    }

    protected override void HandleCollision(Collider other)
    {
        base.HandleCollision(other);
        weaponOwner.ReduceDurability(weaponOwner.WeaponData().GetBreakdownPenalty());

    }
}
