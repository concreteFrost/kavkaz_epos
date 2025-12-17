using UnityEngine;

public class BareHandsWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    private Attack currentAttack;
   
    public IAttackSource AttackSource { get; set; }

    [SerializeField] private WeaponDamageCollider damageCollider;

    #region IWeapon Variables
    public WeaponSO WeaponData() => weaponSO;

    public void SetCurrentAttack(Attack attack)
    {
        currentAttack = attack;
    }

    public Attack GetCurrentAttack() => currentAttack;


    #endregion

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
        var healthDamage = currentAttack.GetFinalHealthDamage(weaponSO.GetBaseDamage());
        var balanceDamage = currentAttack.GetFinalBalanceDamage();
        damageCollider.EnableCollider(healthDamage, balanceDamage, AttackSource.SourceId());
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
