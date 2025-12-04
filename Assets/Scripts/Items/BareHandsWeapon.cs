using UnityEngine;

public class BareHandsWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;

    private float currentDamageAmount;
    public WeaponSO WeaponData() => weaponSO;
    public IAttackSource AttackSource { get; set; }

    [SerializeField] private WeaponDamageCollider damageCollider;


    public void ReduceDurability(float amount)
    {
        
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        currentDamageAmount = weaponSO.damageAmount;
        damageCollider.SetWeapon(this);
    }
    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }

    public void PerformAttack()
    {
        damageCollider.EnableCollider(currentDamageAmount);
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
