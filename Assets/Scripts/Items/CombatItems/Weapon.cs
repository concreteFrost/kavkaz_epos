using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : CombatItem, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    private WeaponAttack currentAttack;

    [SerializeField] private WeaponDamageCollider damageCollider;
    protected WeaponAudioManager audioManager;

    int currentAttackIndex = 0;

    #region IWeapon Contract

    public WeaponSO WeaponData() => weaponSO;
    public WeaponAttack CurrentAttack() => currentAttack;
    public WeaponAttack GetPowerAttack(WeaponAttack attack) => currentAttack = attack;
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

    public override void Init(WeaponData data)
    {
        base.Init(data);

        damageCollider.Init();
        damageCollider.SetWeaponData(this);

        audioManager = new WeaponAudioManager(gameObject);
       

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Upgrade();
        }
    }

    public void Upgrade()
    {
        if (!CanUpgrade())
        {
            Debug.Log("weapon is on max level");
            return;
        }

        data.WeaponLevel++;
    }

    public void Downgrade()
    {
        if (!CanDowngrade())
        {
            Debug.Log("weapon level is on minimum");
            return;
        }

        data.WeaponLevel--;
    }

    public bool CanUpgrade() => data.WeaponLevel < 10;

    public bool CanDowngrade() => data.WeaponLevel > 1;

    public void PlaySwing()
    {
        audioManager.PlaySwing(currentAttack.audioEvent);
    }

    public void PerformAttack()
    {
        if (currentAttack == null || Owner == null) return;

        float baseWeaponDamage = GetWeaponDamageWithLevel();
       
        if (data.durability <= 0) 
            baseWeaponDamage = baseWeaponDamage * 0.5f;

        var ownerStrengthMultiplier = Owner.StatsController.Strength.CurrentMax;

        DamageData damageData = currentAttack.damageData;
        damageData.SetFinalDamage(baseWeaponDamage,ownerStrengthMultiplier);

        damageCollider.EnableCollider(
            damageData,
            Owner.AttackSource.TargetsToIgnore,
            Owner.AttackSource
        );
    }

    private float GetWeaponDamageWithLevel()
    {
        var baseWeaponDamage = WeaponData().GetBaseDamage();

        float weaponMultiplier =
        WeaponUpgradeFormula.GetDamageMultiplier(data.WeaponLevel);

        return baseWeaponDamage *= weaponMultiplier;
    }

    public void CancelAttack()
    {
        damageCollider.DisableCollider();
    }

    public override void AssignToOwner(IInteractor target)
    {
        Owner = target;

        damageCollider.SetWeaponData(this);

        AssignParent(Owner.CombatInventory.GetRightHand());
    }

  

}