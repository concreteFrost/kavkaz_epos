using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Systems/Items/ItemSO")]
public abstract class ItemSO : ScriptableObject
{
    public string id;
    public string itemName;

    private void OnEnable()
    {
        if(id == null)
        {
           id = Guid.NewGuid().ToString();  
        }
    }
}
