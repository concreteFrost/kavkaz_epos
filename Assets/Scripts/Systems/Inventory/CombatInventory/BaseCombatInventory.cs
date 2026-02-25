using UnityEngine;


public abstract class BaseCombatInventory : MonoBehaviour , ICombatInventory
{
    public CombatInventorySO starterSet;

    protected BaseHumanoidAnimatorController animatorController;
    protected CharacterBoneSocket boneSocket;

    protected IHumanoidMeleeCombat combatController;

    #region ICombatInventory Contract

    public Transform GetRightHand() => boneSocket.GetWeaponHolder;
    public Transform GetLeftHand() => boneSocket.GetShieldHolder;

    public abstract void SetWeapon(IWeapon w);

    public abstract void SetShield(IShield w);

    public abstract void ResetWeapon();

    public abstract void ResetShield();

    public IWeapon DefaultWeapon { get; set; } = null;

    public IWeapon CurrentWeapon { get; set; } = null;

    public IShield ShieldWeapon { get; set; } = null;

    public bool CanPickWeapon() => CurrentWeapon.WeaponData().canOverride;

    #endregion


    public IWeapon GetStarterWeapon(ICollector source)
    {
        if (starterSet == null) return null;

        if(starterSet.initialWeapon != null)
        {
            GameObject go = Instantiate(starterSet.initialWeapon);

            Weapon weapon = go.GetComponent<Weapon>();

            weapon.Init(weapon.ItemData);
            weapon.AssignToOwner(source);
     

            return weapon;  
        }

        return null;
    }

    public IShield GetStarterShield(ICollector source)
    {
        if (starterSet == null) return null;

        if (starterSet.initialShield != null)
        {
            GameObject go = Instantiate(starterSet.initialShield);

            Shield shield = go.GetComponent<Shield>();  

            shield.Init(shield.ItemData);   
            shield.AssignToOwner(source);   

            return shield;  
        }

        return null;
    }



}
