using System.Collections.Generic;
using UnityEngine;

public class StaticLootHolder : BaseLootHolder
{
    public override string LootHolderName => "Loot";

    public override string LootInteractionText => "Collect";
    public override ItemInteractionType InteractType() => ItemInteractionType.Item;

    public List<ItemData> lootEntries = new List<ItemData>();

    private bool initialized;

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        if (initialized) return;
        initialized = true;
        base.Init();
        if (HasInteracted)
        {
            DeactivateVisual();
            return;
        }

        foreach (var entry in lootEntries)
        {
            AddItemsToDrop(entry);
        }
    }

    public override void Interact(IInteractor collector)
    {
        base.Interact(collector);
        DeactivateVisual();
      


    }

    public override void LoadLootData(LootState data)
    {
        Init();
        HasInteracted = data.hasCollected;

        if (HasInteracted)
        {
            itemsToDrop.Clear();
            gameObject.SetActive(false);
        }
        else
        {
            itemsToDrop.Clear();
            foreach (var entry in lootEntries) AddItemsToDrop(entry);
            ActivateVisual();
            gameObject.SetActive(true);
        }

    }


}

