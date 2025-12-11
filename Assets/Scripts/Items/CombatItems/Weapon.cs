using UnityEngine;

public class Weapon : CombatItem, IWeapon
{
    public WeaponSO weaponSO;
    private Attack currentAttack;

    [SerializeField] private WeaponDamageCollider damageCollider;

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
        damageCollider.EnableCollider(healthDamage, balanceDamage);
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
            ThrowWeapon();    
        }
    }

    public void ThrowWeapon()
    {

        ResetParent();
        ToggleInteraction(true);

        damageCollider.DisableCollider();
        AttackSource.ResetWeapon();
        AttackSource = null;

    }

   
}
