using System.Collections.Generic;
using UnityEngine;

public class ItemsInitializer : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        var children = GetComponentsInChildren<Item>();
        items.AddRange(children);

        foreach (var child in children)
        {
            child.Init(child.ItemData); 
        }
    }

   
}
