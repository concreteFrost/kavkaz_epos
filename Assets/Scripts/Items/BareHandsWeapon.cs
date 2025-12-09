using UnityEngine;

public class BareHandsWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    public WeaponSO WeaponData() => weaponSO;
    public IAttackSource AttackSource { get; set; }

    [SerializeField] private WeaponDamageCollider damageCollider;

    public void ReduceDurability(float amount) 
    {
        //без имплементации
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        damageCollider.SetWeapon(this);
    }
    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }

    public void PerformAttack()
    {
        var healthDamage = weaponSO.GetHealthDamage();
        var balanceDamage = weaponSO.GetBalanceDamage();    
        damageCollider.EnableCollider(healthDamage,balanceDamage);
    }

    public void SetOwner(IAttackSource source)
    {
        AttackSource = source;  
    }

    public void ThrowWeapon()
    {
        //без имплементации
    }


  
}
