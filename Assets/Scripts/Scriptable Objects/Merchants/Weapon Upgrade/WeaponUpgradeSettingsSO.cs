using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeaponUpgradeTier
{
    public WeaponUpgradeItemSO targetItem;
    public int requiredQuantity;
    public float price;
    public int targetLevel;
}

[CreateAssetMenu(menuName = ScriptablePaths.BASE_PATH + "/Weapon Ugrade Settings", fileName = "weapon_upgrade_settings")]
public class WeaponUpgradeSettingsSO : ScriptableObject
{
    public List<WeaponUpgradeTier> tiers = new List<WeaponUpgradeTier>();
}
