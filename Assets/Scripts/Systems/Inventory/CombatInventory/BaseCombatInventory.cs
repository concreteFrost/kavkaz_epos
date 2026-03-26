using System;
using UnityEngine;


public abstract class BaseCombatInventory : MonoBehaviour, ICombatInventory
{
    public CombatInventorySO starterSet;

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

    public void SetWeapon(IWeapon w)
    {
        if (w == null)
        {
            CurrentWeapon = DefaultWeapon;
            return;
        }

        CurrentWeapon = w;
        combatController.IsWeaponed = true;

        animatorController.OverrideArmed(w);

        WeaponDataUpdated?.Invoke(CurrentWeapon.WeaponData(), CurrentWeapon);

    }

    public void SetShield(IShield w)
    {
        if (w == null) return;
        ShieldWeapon = w;
        Collector.Damagable.Protection = w;

        ShieldUpdated?.Invoke(ShieldWeapon.ShieldData(), ShieldWeapon);
    }


    public void ResetCombatItem(CombatItem i)
    {
        switch (i)
        {
            case Weapon:
                ResetWeapon();
                break;
            case Shield:
                ResetShield();
                break;
            default: break;
        }

    }

    public void ResetWeapon()
    {
        CurrentWeapon = DefaultWeapon;
        combatController.IsWeaponed = false;
        WeaponDataUpdated?.Invoke(CurrentWeapon.WeaponData(), CurrentWeapon);

    }

    public void ResetShield()
    {

        if (ShieldWeapon == null) return;

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
