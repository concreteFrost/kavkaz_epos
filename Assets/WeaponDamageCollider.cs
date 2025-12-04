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
        if (collectedColliders.Contains(other) || weaponOwner == null)
            return;

        collectedColliders.Add(other);
        weaponOwner.ReduceDurability(weaponOwner.WeaponData().breakdownPenalty);

        var damagable = other.GetComponentInChildren<IDamagable>()
                ?? other.GetComponent<IDamagable>();

        if (damagable == null) return;

        var targetId = damagable.SourceId();
        bool isFriend = weaponOwner.AttackSource.TargetIds.Contains(targetId);

        if (!isFriend && targetId != weaponOwner.AttackSource.SourceId())
        {
            // Вместо урона по цели, уменьшаем состояние оружия
            damagable.TakeDamage(currentDamage);
           
        }
    }
}
