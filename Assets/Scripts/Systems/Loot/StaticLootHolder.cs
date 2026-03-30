using System.Collections.Generic;
using UnityEngine;

public class StaticLootHolder : BaseLootHolder
{
    public override ItemInteractionType InteractType() => ItemInteractionType.Item;

    public List<ItemData> guaranteedItems = new List<ItemData>();

    private void Start()
    {
        InitialPosition = transform.position;
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

    public override void Interact(IInteractor collector)
    {
        base.Interact(collector);
        DeactivateVisual();
    }

    public override void LoadLootData(LootState data)
    {
        HasInteracted = data.hasCollected;

        if (HasInteracted)
        {
            itemsToDrop.Clear();
            gameObject.SetActive(false);
        }


    }

   
}
