using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    public Vector3 InitialPosition { get; set; }
    public bool HasInteracted { get; set; }

    public bool CanInteract() => true;

    public ItemInteractionType InteractType() => ItemInteractionType.Item;
    //public ItemSO ItemData { get; set; }

    [SerializeField] protected InteractionCollider interactionCollder;

    public virtual void Init()
    {
        //ItemData = itemData;    

        if(interactionCollder != null)
        {
            interactionCollder.Init();
        }

        InitialPosition = transform.position;
       
    }

    public abstract void Interact(ICollector interractor);



}
