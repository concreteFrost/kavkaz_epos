using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.BASE_PATH + "/Data Base/Weapon Data Base", fileName ="Weapon Data Base")]
public class WeaponDataBaseSO : ScriptableObject
{
    public List<GameObject> weaponData = new List<GameObject>();    

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