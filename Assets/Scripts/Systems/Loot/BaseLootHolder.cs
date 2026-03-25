using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootState
{
    public string lootId;
    public bool hasCollected;
}

public abstract class BaseLootHolder : MonoBehaviour, IInteractable
{
    [HideInInspector] private UniqueId uniqueId;
    public string id;
    public List<ItemData> itemsToDrop = new List<ItemData>();

    public GameObject visual;
    Collider interactionCollider;

    #region IInteractable Contract
    public bool HasInteracted { get; set; }
    public bool CanInteract() => itemsToDrop.Count > 0;
    public Vector3 InitialPosition { get; set; }

    public abstract ItemInteractionType InteractType();

    #endregion


    public virtual void Init()
    {
        uniqueId = GetComponent<UniqueId>();
        id = uniqueId.uniqueId;
        
        itemsToDrop.Clear();
        interactionCollider = GetComponent<Collider>(); 

        ActivateVisual();
    }

    public void ActivateVisual()
    {
        visual.SetActive(true);
        interactionCollider.enabled = true; 
    }
    
    public void DeactivateVisual()
    {
        visual.SetActive(false);
        interactionCollider.enabled = false;
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
        HasInteracted = true;
    }

    public virtual void Interact(ICollector collector)
    {
       
        TransferItemsToCollector(collector);
        //interactionCollider.DisableCollider();
    }

    public virtual LootState SaveLootData()
    {
        return new LootState()
        {
            lootId = id,
            hasCollected = HasInteracted

        };
        
    }

    public abstract void LoadLootData(LootState state);

   
}
