using System.Collections;
using UnityEngine;

public class Weapon : CombatItem, IWeapon
{
    public WeaponSO weaponSO;
    private Attack currentAttack;

    [SerializeField] private WeaponDamageCollider damageCollider;

    [SerializeField] private float minStopVelocity = 1f;
    [SerializeField] private float checkDelay = 0.05f;

    #region IWeapon variables
    public WeaponSO WeaponData() => weaponSO;

    public void SetCurrentAttack(Attack attack)
    {
        currentAttack = attack;
    }

    public Attack GetCurrentAttack() => currentAttack;
    #endregion

    public IAttackSource AttackSource { get; set; }

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
    
        damageCollider.SetWeapon(this);
     
    }

    public void PerformAttack()
    {
        var healthDamage = currentAttack.GetFinalHealthDamage(weaponSO.GetBaseDamage());
        var balanceDamage = currentAttack.GetFinalBalanceDamage();
        damageCollider.EnableCollider(healthDamage, balanceDamage, AttackSource.SourceId());
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
