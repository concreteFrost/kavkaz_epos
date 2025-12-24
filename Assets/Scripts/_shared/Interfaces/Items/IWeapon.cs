
using UnityEngine;

public interface IWeapon 
{
    public WeaponSO WeaponData();
    public void PerformAttack();
    public void CancelAttack();
    public void DropWeapon();
    public void ThrowWeapon(Transform from, float force);
    public void ReduceDurability(float amount);
    public void SetCurrentAttack(Attack attack);
    public Attack GetCurrentAttack();

}
