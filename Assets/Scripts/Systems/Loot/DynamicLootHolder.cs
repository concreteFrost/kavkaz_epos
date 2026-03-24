using System.Collections.Generic;
using UnityEngine;


public class DynamicLootHolder : BaseLootHolder
{

    public Vector3 currentPoisiton;

    public override void Init()
    {
        base.Init();        
    }

    public void AddItemsFromDistributer(List<ItemData> list)
    {
        itemsToDrop.Clear();
        itemsToDrop.AddRange(list);
    }
    public override void PickUp(ICollector collector)
    {
        base.PickUp(collector);
        Destroy(this.gameObject); 
    }

}
