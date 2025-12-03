using UnityEngine;

public class BareHandsWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    public WeaponSO WeaponData() => weaponSO;
    protected string owner { get; set; }
    public string Owner { get => owner; set => owner = value; }

    [SerializeField] private DamageCollider damageCollider;

    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }

    public void PerformAttack()
    {
        damageCollider.EnableCollider();
    }

    public void ThrowWeapon()
    {
        //без имплементации
    }


  
}
