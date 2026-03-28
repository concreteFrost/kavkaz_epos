using System;
using UnityEngine;


public class HumanoidWeaponSetter : MonoBehaviour, IWeaponSetter
{
    [Header("Bare Hands Settings")]
    [SerializeField] private MeleeData meleeData;

    protected BaseHumanoidAnimatorController animatorController;
    protected CharacterBoneSocket boneSocket;

    protected IHumanoidMeleeCombat combatController;

    public bool enableWeponBreakdown;

    public Action<ItemSO, IBreakable> WeaponDataUpdated;
    public Action<ItemSO, IBreakable> ShieldUpdated;

    #region ICombatInventory Contract
    public Transform GetRightHand() => boneSocket.GetWeaponHolder;
    public Transform GetLeftHand() => boneSocket.GetShieldHolder;

    public IWeapon DefaultWeapon { get; set; } = null;

    public IWeapon CurrentWeapon { get; set; } = null;

    public IShield ShieldWeapon { get; set; } = null;

    public ICollector Collector;

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (CurrentWeapon != null)
            {
                CurrentWeapon.ReduceDurability(10f);
            }
        }
    }

    public void Init(
        CharacterBoneSocket boneSocket,
        BaseHumanoidAnimatorController animatorController,
        IHumanoidMeleeCombat combatController,
        ICollector collector,
        bool enableWeaponBreakdown)
    {

        this.boneSocket = boneSocket;
        this.combatController = combatController;
        this.animatorController = animatorController;
        this.Collector = collector;
        this.enableWeponBreakdown = enableWeaponBreakdown;

        DefaultWeapon = InitializeBarehands(collector);
        SetWeapon(DefaultWeapon);

    }

    private IWeapon InitializeBarehands(ICollector attackSource)
    {

        meleeData.leftDamageCollider = boneSocket.GetLeftMeleeSocket.GetComponent<WeaponDamageCollider>();
        meleeData.rightDamageCollider = boneSocket.GetRightMeleeSocket.GetComponent<WeaponDamageCollider>();
        var bareHands = new MeleeWeapon();
        bareHands.Init(meleeData, attackSource);
        return bareHands;

    }

    public void ResetAllCombatItems()
    {
        ResetWeapon();
        ResetShield();  
    }

    public void HandleSetCombatItem(ICombatItem item)
    {
        if(item is IWeapon)
        {
            ResetWeapon();
            SetWeapon((IWeapon)item);
            return;
        }

        if(item is IShield)
        {
            ResetShield();
            SetShield((IShield)item);
        }

    }

    public void HandleResetCombatItem(string id)
    {
        if (CurrentWeapon.GetItemData().instanceId == id)
            ResetWeapon();

        if(ShieldWeapon != null && ShieldWeapon.GetItemData().instanceId == id)
            ResetShield();
       
    }

    private void SetWeapon(IWeapon w)
    {

        if (w == null)
        {
            CurrentWeapon = DefaultWeapon;
            return;
        }

        CurrentWeapon = w;
        
        CurrentWeapon.SetEquiped(true);
        CurrentWeapon.AssignToOwner(Collector);
     
        combatController.IsWeaponed = true;

        animatorController.OverrideArmed(w);

        WeaponDataUpdated?.Invoke(CurrentWeapon.WeaponData(), CurrentWeapon);

        if (CurrentWeapon.WeaponData().weaponType == WeaponType.TwoHands)
        {
            //сбрасываем щит так как оружие двуручное
            ResetShield();
        }

    }

    private void SetShield(IShield w)
    {

        if (w == null) return;
        if (CurrentWeapon.WeaponData().weaponType == WeaponType.TwoHands) return;
  
        ShieldWeapon = w;

        ShieldWeapon.SetEquiped(true);
        Collector.Damagable.Protection = w;
        ShieldWeapon.AssignToOwner(Collector);
        ShieldUpdated?.Invoke(ShieldWeapon.ShieldData(), ShieldWeapon);
    }

    private void ResetWeapon()
    {
        CurrentWeapon.SetEquiped(false);
        CurrentWeapon = DefaultWeapon;
        combatController.IsWeaponed = false;
        WeaponDataUpdated?.Invoke(CurrentWeapon.WeaponData(), CurrentWeapon);

    }

    private void ResetShield()
    {

        if (ShieldWeapon == null) return;


        ShieldWeapon.SetEquiped(false);
        ShieldUpdated?.Invoke(ShieldWeapon.ShieldData(), null);
        Collector.Damagable.Protection = null;
        ShieldWeapon = null;
        combatController.IsShieldRaised = false;

    }

    // для UI обновления
    public void GetCurrentWeaponData()
    {
        WeaponDataUpdated?.Invoke(CurrentWeapon.WeaponData(), CurrentWeapon);
    }

    //для Ui обновления на старте
    public void GetCurrentShieldData()
    {
        if (ShieldWeapon == null)
        {
            ShieldUpdated?.Invoke(null, ShieldWeapon);
            return;
        }

        ShieldUpdated?.Invoke(ShieldWeapon.ShieldData(), ShieldWeapon);
    }



}
