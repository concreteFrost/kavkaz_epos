using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItemEntry
{
    public ItemSO itemSO;
    [Min(1)] public int quantity = 1;
    [Min(1)] public int weaponLevel = 1;
    [Range(0f, 100f)] public float durability = 100f;
}
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
    protected List<ItemData> itemsToDrop = new List<ItemData>();

    public GameObject visual;
    Collider interactionCollider;

    #region IInteractable Contract
    public string InteractionName() => LootHolderName;
    public string ActionText() => LootInteractionText;
    public bool HasInteracted { get; set; }
    public bool CanInteract() => itemsToDrop.Count > 0;

    public abstract ItemInteractionType InteractType();

    #endregion

    public abstract string LootHolderName { get;  }
    public abstract string LootInteractionText {  get; }

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
        AddItemsToDrop(new LootItemEntry { itemSO = itemSO, quantity = quantity });
    }

    protected void AddItemsToDrop(LootItemEntry entry)
    {
        if (entry == null || entry.itemSO == null) return;

        ItemData data;
        if (entry.itemSO is CombatItemSO)
        {
            data = entry.itemSO is ShieldSO ? new ShieldData { instanceId = Guid.NewGuid().ToString(), durability = entry.durability } : new WeaponData
            {
                instanceId = Guid.NewGuid().ToString(),
                durability = entry.durability,
                WeaponLevel = entry.weaponLevel
            };
        }
        else if (entry.itemSO is SpellProjectileSO)
        {
            data = new SpellData();
        }
        else if (entry.itemSO is ConsumableItemSO)
        {
            data = new ConsumableData();
        }
        else
        {
            data = new ItemData();
        }

        data.itemSO = entry.itemSO;
        data.quantity = entry.quantity;
        itemsToDrop.Add(data);
    }


    public void TransferItemsToCollector(IInteractor collector)
    {
        foreach(var item in itemsToDrop)
        {
            collector.DistributeItemToInventory(item);
        }

        itemsToDrop.Clear();
        HasInteracted = true;
    }

    public virtual void Interact(IInteractor collector)
    {
       
        TransferItemsToCollector(collector);
        LootManager.StaticLootDataUpdated?.Invoke();
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
