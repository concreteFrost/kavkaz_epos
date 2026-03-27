using NUnit.Framework;
using UnityEngine;
using Zenject;

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

        this.leftDamageCollider.Init();
        this.leftDamageCollider.SetWeaponData(weapon);
        //this.leftDamageCollider.SetDamageSource(source);

        this.rightDamageCollider.Init();
        this.rightDamageCollider.SetWeaponData(weapon);
        //this.rightDamageCollider.SetDamageSource(source);
    
    }

    public void SetCurrentCollider(WeaponAttack attack)
    {
        if (attack.fromHand == FromHand.left)
            current = leftDamageCollider;
        if (attack.fromHand == FromHand.right)
            current = rightDamageCollider;
    }

    public void PerformAttack(DamageData damageData ,IAttackSource attackSource)
    {
        current.EnableCollider(damageData, attackSource.TargetsToIgnore, attackSource.Source());
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

    private WeaponAttack currentAttack;

    public WeaponAttack GetPowerAttack(WeaponAttack attack) => currentAttack = attack;


    int currentAttackIndex = 0;

    #region IWeapon Contract

    public ICollector Owner { get; set; }
    public bool IsBreakdownEnabled { get; set; } = true;
    public bool IsBroken { get; set; }= false;

    public WeaponSO WeaponData() => weaponSO;

    public WeaponAttack CurrentAttack() => currentAttack;

    public void SetCurrentAttack(WeaponAttack attack) => currentAttack = attack;

    public float GetDurability() => 0; // заглушка

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
    public void Init(MeleeData meleeData,ICollector owner)
    {
        this.weaponSO = meleeData.barehandsData;
        this.Owner = owner;

        this.meleeData = new MeleeData();
        this.meleeData.Init(meleeData, this, this.Owner.AttackSource.Source());

    }


    public void CancelAttack()
    {
        meleeData.CancelAttack();
    }

    public void PerformAttack()
    {

        if (currentAttack == null)
        {
            Debug.Log("no current attack assigned");
            return;

        }

        var baseWeaponDamage = WeaponData().GetBaseDamage();
        var ownerStrengthMultiplier = Owner.StatsController.Strength.CurrentMax;

        //DamageData damageData = new DamageData()
        //{
        //    damageMultiplier = currentAttack.GetFinalDamage(baseWeaponDamage, ownerStrengthMultiplier),
        //    balanceDamageType = currentAttack.damageData.balanceDamageType,
        //    impactForce = currentAttack.damageData.impactForce,
        //    statusEffectData = currentAttack.damageData.statusEffectData

        //};
        currentAttack.damageData.SetFinalDamage(baseWeaponDamage, ownerStrengthMultiplier);
        meleeData.SetCurrentCollider(currentAttack);
        meleeData.PerformAttack(currentAttack.damageData, Owner.AttackSource);
    }

    #region Unused IWeapon Contract Methods

    public void ReduceDurability(float amount)
    {
        //��� �������������
    }

    public void IncreaseDurability(float amount)
    {

    }

    public void AssignToOwner(ICollector source)
    {
        //��� �������������
    }

    public void SetBreakdownEnabled(bool isEnabled)
    {
       //
    }

    public void SetEquiped(bool equiped)
    {

    }


    #endregion



}