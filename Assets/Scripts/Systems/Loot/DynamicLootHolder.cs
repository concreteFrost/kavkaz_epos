using System.Collections.Generic;
using UnityEngine;


public class DynamicLootHolder : BaseLootHolder
{

    public Vector3 currentPoisiton;

    public override ItemInteractionType InteractType()=> ItemInteractionType.Item; 

    public override void Init()
    {
        base.Init();        
    }

    public void AddItemsFromDistributer(List<ItemData> list)
    {
        itemsToDrop.Clear();
        itemsToDrop.AddRange(list);
    }
    public override void Interact(ICollector collector)
    {
        base.Interact(collector);
        Destroy(this.gameObject); 
    }

    public override void LoadLootData(LootState state)
    {
        
    }

  

}
