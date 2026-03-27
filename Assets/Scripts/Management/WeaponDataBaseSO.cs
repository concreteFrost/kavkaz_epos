using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.BASE_PATH + "/Data Base/Weapon Data Base", fileName ="Weapon Data Base")]
public class WeaponDataBaseSO : ScriptableObject
{
    public List<GameObject> weaponData = new List<GameObject>(); 
    
    public List<CombatItemSO> GetAllWeapons()
    {
        List<CombatItemSO> combatItems = new List<CombatItemSO>();
        foreach (GameObject weapon in weaponData)
        {
            if (weapon.TryGetComponent(out Weapon w))
            {
                combatItems.Add(w.WeaponData());
            }
            else if (weapon.TryGetComponent(out Shield s))
            {
                combatItems.Add(s.ShieldData());
            }
        }

        return combatItems; 
    }

    public GameObject Get(string id)
    {

        foreach (var weapon in weaponData)
        {
            if(weapon.TryGetComponent(out Weapon w))
            {
                if(w.WeaponData().id == id)
                {
                    return w.gameObject;
                }
            }
            else if(weapon.TryGetComponent(out Shield s))
            {
                if(s.ShieldData().id == id)
                {
                    return s.gameObject;
                }
            }
        }

        return null;
    }
}