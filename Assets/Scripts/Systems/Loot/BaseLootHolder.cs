using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLootHolder : MonoBehaviour, IPickable
{

    public List<ItemData> itemsToDrop = new List<ItemData>();

    public bool HasInteracted { get; set; }
    public bool CanInteract() => itemsToDrop.Count > 0;
    public Vector3 InitialPosition { get; set; }

    //private void Start()
    //{
    //    Init();
    //}

    public virtual void Init()
    {
        itemsToDrop.Clear();
        InitialPosition = transform.position;   
    }

    protected void AddItemsToDrop(ItemSO itemSO, int quantity)
    {
        ItemData data = new ItemData();
        data.itemSO =itemSO;
        data.quantity = quantity;
        itemsToDrop.Add(data);
    }


    public void TransferItemsToCollector(ICollector collector)
    {
        foreach(var item in itemsToDrop)
        {
            collector.DistributeItemToInventory(item);
        }

        itemsToDrop.Clear();
    }

    public virtual void PickUp(ICollector collector)
    {
        
        TransferItemsToCollector(collector);
        //interactionCollider.DisableCollider();
    }

   
}
