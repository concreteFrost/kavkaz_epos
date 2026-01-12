using System.Collections;
using UnityEngine;

public class Weapon : CombatItem, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    private Attack currentAttack;

    [SerializeField] private WeaponDamageCollider damageCollider;
    public IAttackSource AttackSource { get; set; }

    private float minStopVelocity = 1.7f;

    int currentAttackIndex = 0;

    #region IWeapon Contract
    public WeaponSO WeaponData() => weaponSO;
    public Attack CurrentAttack() => currentAttack;
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
    void Start()
    {
        Init(weaponSO);
    }

    protected override void Init(ItemSO itemData)
    {

        base.Init(itemData);
        ToggleInteraction(true);
       
        breakdownThreshold = 100f;
    
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
            AttackSource.TargetsToIgnore
        );
    }

    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }
    public override void PickUp(IAttackSource target)
    {

        if(breakdownThreshold <= 0)
        {
            Debug.Log("this weapon is broken");
            return;
        }

        if (!target.CurrentWeapon.WeaponData().canOverride)
            return;

        AttackSource = target;
        damageCollider.SetWeaponData(this);
        damageCollider.SetDamageSource(AttackSource.SourcePosition());

        AssignParent(target.GetRightHand());
        ToggleInteraction(false);

        
        target.SetWeapon(this); 

    }

    public void ReduceDurability(float amount)
    {
        breakdownThreshold -= amount;   

        if(breakdownThreshold <= 0)
        {
            AttackSource.ResetWeapon();
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

        ResetOwner();

        rb.AddForce(from.forward * force, ForceMode.Impulse);
 
        StartCoroutine(ThrowCoroutine(0.1f)); 
        StartCoroutine(DisableColliderWhenStopped());

    }

    private void ResetOwner()
    {
        AttackSource.ResetWeapon();
        AttackSource = null;
        damageCollider.SetDamageSource(null);
    }

    IEnumerator ThrowCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay); //задержка чтобы не попадать по владельцу оружия

        var healthDamage = weaponSO.GetBaseDamage();
        var balanceDamage = 0.1f;
        damageCollider.EnableCollider(healthDamage,balanceDamage,null);

        yield return null;
    }

    IEnumerator DisableColliderWhenStopped()
    {
        // ждём, пока оружие реально начнёт двигаться
        yield return new WaitUntil(() => rb.linearVelocity.sqrMagnitude > 0.1f);
       

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
