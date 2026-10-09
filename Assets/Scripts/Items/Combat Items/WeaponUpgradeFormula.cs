using UnityEngine;

public class WeaponUpgradeFormula 
{
    public static float GetDamageMultiplier(int weaponLevel)
    {
        const float firstBonus = 0.10f;
        const float diminishFactor = 0.7314f;

        weaponLevel = Mathf.Max(1, weaponLevel);

        float bonus =
            firstBonus *
            (1f - Mathf.Pow(diminishFactor, weaponLevel - 1)) /
            (1f - diminishFactor);

        return 1f + bonus;
    }
}
