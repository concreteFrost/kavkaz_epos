using UnityEngine;

public class Chest : StaticLootHolder
{
    Animator animator;
    public bool isOpened;

    [SerializeField] private ParticleSystem lootParticles;

    private ItemInteractionType interactionType = ItemInteractionType.Chest;

    public override ItemInteractionType InteractType() => interactionType;

    public override string LootHolderName => "Chest";

    public override string LootInteractionText => "Open";
    public override void Init()
    {
        base.Init();    

        animator = GetComponent<Animator>();
        lootParticles.Stop();   
      
    }


    public override void Interact(IInteractor collector)
    {
        if (!isOpened)
        {
            OpenChest();
            interactionType = ItemInteractionType.Item;
        }
        else
        {
            
            lootParticles.Stop();
            TransferItemsToCollector(collector);
        }
        
    }

    public void OpenChest()
    {
        animator.SetBool("Open", true);
        isOpened = true;

        if (!HasInteracted) lootParticles.Play();
    }

    public void CloseChest()
    {
        animator.SetBool("Open", false);
        isOpened = false;

        lootParticles.Stop();
        
    }

    public override void LoadLootData(LootState data)
    {
        HasInteracted = data.hasCollected;

        if (data.hasCollected)
        {
            OpenChest();
            lootParticles.Stop();
            itemsToDrop.Clear();

        }
        else
        {
            CloseChest();  
        }
    }
}
