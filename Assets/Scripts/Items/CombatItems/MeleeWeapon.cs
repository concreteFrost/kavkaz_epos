using NUnit.Framework;
using UnityEngine;

[System.Serializable]
public class MeleeData
{

    public WeaponSO barehandsData;

    public WeaponDamageCollider leftDamageCollider;
    public WeaponDamageCollider rightDamageCollider;

    [HideInInspector] public WeaponDamageCollider current;

    public void Init(MeleeData data, IWeapon weapon, Transform source)
    {
        this.barehandsData = data.barehandsData;

        this.leftDamageCollider = data.leftDamageCollider;
        this.rightDamageCollider = data.rightDamageCollider;

        this.leftDamageCollider.SetWeaponData(weapon);
        this.leftDamageCollider.SetDamageSource(source);

        this.rightDamageCollider.SetWeaponData(weapon);
        this.rightDamageCollider.SetDamageSource(source);
    }

    public void SetCurrentCollider(Attack attack)
    {
        if (attack.fromHand == FromHand.left)
            current = leftDamageCollider;
        if(attack.fromHand == FromHand.right)
            current = rightDamageCollider;  
    }

    public void PerformAttack(float healthDamage, BalanceDamageType balanceDamage, ICombatInventory attackSource)
    {
        current.EnableCollider(healthDamage, balanceDamage, attackSource.TargetsToIgnore);
    }

    public void CancelAttack()
    {
        if (current == null) return;

        current.DisableCollider();
    }
  
}

public class MeleeWeapon : IWeapon
{
    private WeaponSO weaponSO;
    private MeleeData meleeData;

    private Attack currentAttack;

    public Attack GetPowerAttack(Attack attack)=>currentAttack=attack;  
    public ICombatInventory AttackSource { get; set; }

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

    public void Init(MeleeData meleeData, ICombatInventory source)
    {
        this.weaponSO = meleeData.barehandsData;
        this.AttackSource = source; 

        this.meleeData = new MeleeData();
        this.meleeData.Init(meleeData,this, AttackSource.SourcePosition());
        
  
        
    }
    public void CancelAttack()
    {
        meleeData.CancelAttack();
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

        meleeData.SetCurrentCollider(currentAttack);
        meleeData.PerformAttack(healthDamage, balanceDamage, AttackSource);
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
