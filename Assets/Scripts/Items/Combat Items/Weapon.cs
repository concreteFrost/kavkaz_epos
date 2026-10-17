using UnityEngine;

public class Weapon : CombatItem, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;
    private WeaponAttack currentAttack;

    [SerializeField] private WeaponDamageCollider damageCollider;
    [Tooltip("Необязательный след клинка, видимый во время нанесения удара.")]
    [SerializeField] private TrailRenderer attackTrace;
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

    public override void Init(CombatItemData data)
    {
       
        base.Init(data);
        SetAttackTrace(false, true);

        damageCollider.Init();
        damageCollider.SetWeaponData(this);

        audioManager = new WeaponAudioManager(gameObject);
       

    }
   

    public void PlaySwing()
    {
        audioManager.PlaySwing(currentAttack.audioEvent);
    }

    public void PerformAttack()
    {
        if (currentAttack == null || Owner == null) return;

        var weaponData = (WeaponData)data;
        float baseWeaponDamage = weaponData.GetWeaponDamageWithLevel(weaponData.WeaponLevel);
       
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
        SetAttackTrace(true, true);
    }



    public void CancelAttack()
    {
        damageCollider.DisableCollider();
        SetAttackTrace(false, false);
    }

    public override void SetEquiped(bool equiped)
    {
        base.SetEquiped(equiped);
        SetAttackTrace(false, true);
    }

    private void OnDisable()
    {
        SetAttackTrace(false, true);
    }

    private void SetAttackTrace(bool emitting, bool clear)
    {
        if (attackTrace == null) return;

        if (clear) attackTrace.Clear();
        attackTrace.emitting = emitting;
    }

    public override void AssignToOwner(IInteractor target)
    {
        Owner = target;

        damageCollider.SetWeaponData(this);

        AssignParent(Owner.WeaponSetter.GetRightHand());
    }

  

}
