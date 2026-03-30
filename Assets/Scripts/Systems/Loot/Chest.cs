using UnityEngine;

public class Chest : StaticLootHolder
{
    Animator animator;
    public bool isOpened;

    [SerializeField] private ParticleSystem lootParticles;

    private ItemInteractionType interactionType = ItemInteractionType.Chest;

    public override ItemInteractionType InteractType() => interactionType;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();    

        animator = GetComponent<Animator>();
        lootParticles.Stop();   
        CloseChest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!isOpened) OpenChest();
            else CloseChest();
        }
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
