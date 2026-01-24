using System.Collections;
using UnityEngine;

public class Weapon : CombatItem, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    private Attack currentAttack;

    [SerializeField] private WeaponDamageCollider damageCollider;
    
    public ICollector Owner;

    private float minStopVelocity = 1.7f;

    int currentAttackIndex = 0;

    #region IWeapon Contract
    public WeaponSO WeaponData() => weaponSO;
    public Attack CurrentAttack() => currentAttack;

    public Attack GetPowerAttack(Attack attack)=> currentAttack = attack;
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    Init(weaponSO);
    //}

    public override void Init(ItemSO itemData)
    {

        base.Init(itemData);

        ToggleInteraction(true);
       
        damageCollider.SetWeaponData(this);
     
    }

    public void PerformAttack()
    {
        if (currentAttack == null) return;

        var healthDamage =
            currentAttack.GetFinalHealthDamage(weaponSO.GetBaseDamage());

        var balanceDamage =
            currentAttack.GetFinalBalanceDamage();

        damageCollider.EnableCollider(
            healthDamage,
            balanceDamage,
            Owner.AttackSource.TargetsToIgnore
        );
    }

    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }
    public override void PickUp(ICollector target)
    {
        if (!target.AttackSource.CanPickWeapon()) return;

        if(breakdownThreshold <= 0)
        {
            Debug.Log("this weapon is broken");
            return;
        }
        
        AssignToOwner(target);  

    }

    public void AssignToOwner(ICollector target)
    {
        Owner = target;

        damageCollider.SetWeaponData(this);
        damageCollider.SetDamageSource(Owner.AttackSource.SourcePosition());

        AssignParent(Owner.AttackSource.GetRightHand());
        ToggleInteraction(false);


        target.AttackSource.SetWeapon(this);
    }

    public void ReduceDurability(float amount)
    {
        breakdownThreshold -= amount;   

        if(breakdownThreshold <= 0)
        {
            Owner.AttackSource.ResetWeapon();
            DropWeapon();   
        }
    }

    public void DropWeapon()
    {
        ResetParent();
        ResetOwner();
        ToggleInteraction(true);

       
    }

    public void ThrowWeapon(Transform from, float force)
    {
        ResetParent();
        ResetOwner();

        ToggleInteraction(true);

        rb.AddForce(from.forward * force, ForceMode.Impulse);
 
        StartCoroutine(ThrowCoroutine(0.25f)); 
        StartCoroutine(DisableColliderWhenStopped());

    }

    private void ResetOwner()
    {
        Owner.AttackSource.ResetWeapon();
        Owner = null;
        damageCollider.SetDamageSource(null);
    }

    IEnumerator ThrowCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay); //задержка чтобы не попадать по владельцу оружия

        var healthDamage = weaponSO.GetBaseDamage();
        
        damageCollider.EnableCollider(healthDamage,BalanceDamageType.Extreme,null);

        yield return null;
    }

    IEnumerator DisableColliderWhenStopped()
    {
        // ждём, пока оружие реально начнёт двигаться
        yield return new WaitUntil(() => rb.linearVelocity.sqrMagnitude > 0.15f);
       

        while (true)
        {
            if (rb.linearVelocity.magnitude < minStopVelocity)
            {
                damageCollider.DisableCollider();
               
                yield break;
            }

            yield return null;
        }


    }


}
