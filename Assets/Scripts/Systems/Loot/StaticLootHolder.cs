using System.Collections.Generic;
using UnityEngine;

public class StaticLootHolder : BaseLootHolder
{

    public List<ItemData> guaranteedItems = new List<ItemData>();

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();    
        foreach(var i in guaranteedItems)
        {
            AddItemsToDrop(i.itemSO, i.quantity);
        }
    }
}
