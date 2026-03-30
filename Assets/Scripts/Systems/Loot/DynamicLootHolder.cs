using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicLootHolder : BaseLootHolder
{
    private string instanceId; //генерируется при дропе лута чтобы корректно сохранять/загружать данные

    public override ItemInteractionType InteractType()=> ItemInteractionType.Item; 

    public override void Init()
    {
        base.Init();
       
    }

    public void SetInstanceId(string instanceId)=>this.instanceId = instanceId; 

    public void AddItemsFromDistributer(List<ItemData> data)
    {
        itemsToDrop.Clear();
        itemsToDrop.AddRange(data);
      

    }
    public override void Interact(IInteractor collector)
    {
        base.Interact(collector);

        DynamicLootManager.LootCollected?.Invoke(instanceId);
        Destroy(gameObject);
    }

    public override void LoadLootData(LootState state)
    {
        
    }

  

}
