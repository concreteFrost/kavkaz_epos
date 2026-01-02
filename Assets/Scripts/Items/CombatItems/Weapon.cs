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
    
        damageCollider.SetWeapon(this, AttackSource);
     
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
            AttackSource.SourceId()
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
        ToggleInteraction(true);

        damageCollider.DisableCollider();
        AttackSource.ResetWeapon();
        AttackSource = null;
    }

    public void ThrowWeapon(Transform from, float force)
    {


        ResetParent();
        ToggleInteraction(true);

        AttackSource.ResetWeapon();
        AttackSource = null;

        rb.AddForce(from.forward * force, ForceMode.Impulse);
 
        StartCoroutine(ThrowCoroutine(0.1f)); 
        StartCoroutine(DisableColliderWhenStopped());

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
        Debug.Log("weapon flying");

        while (true)
        {
            if (rb.linearVelocity.magnitude < minStopVelocity)
            {
                damageCollider.DisableCollider();
                Debug.Log("weapon landed");
                yield break;
            }

            yield return null;
        }


    }


}
