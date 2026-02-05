using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Inventory", menuName = ScriptablePaths.COMBAT_PATH + "/Inventory/Combat Inventory")]
public class CombatInventorySO : ScriptableObject
{
    public GameObject initialWeapon;
    public GameObject initialShield;
}
