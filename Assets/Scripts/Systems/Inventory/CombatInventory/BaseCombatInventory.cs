using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class BaseCombatInventory : MonoBehaviour , ICombatInventory
{
    [SerializeField] protected CombatInventorySO starterSet;

    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform leftHand;

    protected BaseHumanoidAnimatorController animatorController;

    protected IHumanoidCombat combatController;

    #region ICombatInventory Contract

    public Transform GetRightHand() => rightHand;
    public Transform GetLeftHand() => leftHand;

    public abstract void SetWeapon(IWeapon w);

    public abstract void SetShield(IShield w);

    public abstract void ResetWeapon();

    public abstract void ResetShield();

    public IWeapon DefaultWeapon { get; set; } = null;

    public IWeapon CurrentWeapon { get; set; } = null;

    public IShield ShieldWeapon { get; set; } = null;

    public bool CanPickWeapon() => CurrentWeapon.WeaponData().canOverride;

    #endregion


    public IWeapon GetStarterWeapon(ICollector collector)
    {
        if (starterSet == null) return null;

        if(starterSet.initialWeapon != null)
        {
            GameObject go = Instantiate(starterSet.initialWeapon);

            Weapon weapon = go.GetComponent<Weapon>();

            weapon.Init(weapon.ItemData);
            weapon.AssignToOwner(collector);

            return weapon;  
        }

        return null;
    }

    public IShield GetStarterShield(ICollector collector)
    {
        if (starterSet == null) return null;

        if (starterSet.initialShield != null)
        {
            GameObject go = Instantiate(starterSet.initialShield);

            Shield shield = go.GetComponent<Shield>();  

            shield.Init(shield.ItemData);   
            shield.AssignToOwner(collector);   

            return shield;  
        }

        return null;
    }



}
