using UnityEngine;

public class BareHandsWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    private Attack currentAttack;
    
    public IAttackSource AttackSource { get; set; }

    [SerializeField] private WeaponDamageCollider damageCollider;

    int currentAttackIndex = 0;

    #region IWeapon Contract
    public WeaponSO WeaponData() => weaponSO;

    public Attack CurrentAttack() => currentAttack;

    public void SetCurrentAttack(Attack attack) => currentAttack = attack;  

    public void SelectAttack(int index)
    {
        var list = weaponSO.attackSet.attackList;

        if (index < 0 || index >= list.Count)
        {
            currentAttackIndex = 0;
        }
        else
        {
            currentAttackIndex = index;
        }

        currentAttack = list[currentAttackIndex];
    }

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
        damageCollider.SetWeapon(this, AttackSource);
    }
    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }

    public void PerformAttack()
    {

        if(currentAttack == null)
        {
            Debug.Log("no current attack assigned");
            return;

        }
        var healthDamage = currentAttack.GetFinalHealthDamage(weaponSO.GetBaseDamage());
        var balanceDamage = currentAttack.GetFinalBalanceDamage();
        damageCollider.EnableCollider(healthDamage, balanceDamage, AttackSource.SourceId());
    }

    public void SetOwner(IAttackSource source)
    {
        AttackSource = source;  
    }

    public void DropWeapon()
    {
        //без имплементации
    }

    public void ThrowWeapon(Transform from, float force)
    {
        //без имплементации
    }

   
}
