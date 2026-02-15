using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "Item", menuName = ScriptablePaths.ITEMS_PATH + "/Item")]
public abstract class ItemSO : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite itemImage;

    private void OnEnable()
    {
        if(id == null)
        {
           id = Guid.NewGuid().ToString();  
        }
    }
}
