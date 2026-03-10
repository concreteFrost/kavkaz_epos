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

    public abstract void ResetCombatItem(CombatItem i);

    public IWeapon DefaultWeapon { get; set; } = null;

    public IWeapon CurrentWeapon { get; set; } = null;

    public IShield ShieldWeapon { get; set; } = null;

    protected ICollector Collector;

    #endregion


    public IWeapon GetStarterWeapon(ICollector source , bool predictWeaponDamage)
    {
        if (starterSet == null) return null;

        if(starterSet.initialWeapon != null)
        {
            GameObject go = Instantiate(starterSet.initialWeapon);

            Weapon weapon = go.GetComponent<Weapon>();

            weapon.Init(weapon.ItemData);
            weapon.AssignToOwner(source);
            weapon.SetBreakdownEnabled(predictWeaponDamage);

            return weapon;  
        }

        return null;
    }

    public IShield GetStarterShield(ICollector source, bool predictWeaponDamage)
    {
        if (starterSet == null) return null;

        if (starterSet.initialShield != null)
        {
            GameObject go = Instantiate(starterSet.initialShield);

            Shield shield = go.GetComponent<Shield>();  

            shield.Init(shield.ItemData);   
            shield.AssignToOwner(source);
            shield.SetBreakdownEnabled(predictWeaponDamage);

            return shield;  
        }

        return null;
    }



}
