using UnityEngine;

public interface IWeapon 
{
    public WeaponSO WeaponData();
    public void PerformAttack();
    public void CancelAttack();
    public void ThrowWeapon();

    public string Owner { get; set; }   
}
