using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public List<CombatItem> items = new List<CombatItem>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        var children = GetComponentsInChildren<CombatItem>();
        items.AddRange(children);

        foreach (var child in children)
        {
            child.Init(); 
        }
    }

   
}
