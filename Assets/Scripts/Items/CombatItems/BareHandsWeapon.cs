using UnityEngine;

public class BareHandsWeapon : IWeapon
{
    private WeaponSO weaponSO;
    private WeaponDamageCollider damageCollider;

    private Attack currentAttack;
    public IAttackSource AttackSource { get; set; }

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

    //private void Start()
    //{
    //    Init();
    //}

    public void Init(WeaponSO weaponSO, WeaponDamageCollider damageCollider, IAttackSource source)
    {
        this.weaponSO = weaponSO;
        this.damageCollider = damageCollider;  
        
        this.AttackSource = source; 

        this.damageCollider.SetWeaponData(this);
        this.damageCollider.SetDamageSource(AttackSource.SourcePosition());
        
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
        damageCollider.EnableCollider(healthDamage, balanceDamage, AttackSource.TargetsToIgnore);
    }


    public void DropWeapon()
    {
        //без имплементации
    }

    public void ThrowWeapon(Transform from, float force)
    {
        //без имплементации
    }

    public void ReduceDurability(float amount)
    {
        //без имплементации
    }

    public void AssignToOwner(ICollector source)
    {
        //без имплементации
    }


}
