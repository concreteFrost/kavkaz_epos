using UnityEngine;

public class WeaponDamageCollider : DamageCollider
{
    public IWeapon weaponOwner; // Ссылка на оружие, состояние которого будет уменьшаться

    public void SetWeapon(IWeapon _weapon)
    {
        weaponOwner = _weapon;
    }

    protected override void HandleCollision(Collider other, float damage)
    {
        if (attackInterrupted)
            return;


        if (collectedColliders.Contains(other))
            return;

        weaponOwner.ReduceDurability(weaponOwner.WeaponData().breakdownPenalty);

        if (TargetHasShield(other)) return;

        PerformNormalDamage(other);
    }
}
