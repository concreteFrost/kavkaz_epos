using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatInventory", menuName = "Scriptable Systems/Combat/Inventory/CombatInventory")]
public class CombatInventorySO : ScriptableObject
{
    public GameObject initialWeapon;
    public GameObject initialShield;
}
