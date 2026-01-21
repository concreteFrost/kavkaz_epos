using UnityEngine;

public abstract class Item : MonoBehaviour, IPickable 
{

    public bool IsPicked { get; set; }

    public abstract void PickUp(ICollector s);

    public ItemSO ItemData { get; set; }

    protected GameObject physicalInstance;

    [SerializeField] protected InteractionCollider interactionCollder;

    public virtual void Init(ItemSO itemData)
    {
        ItemData = itemData;    

        if(interactionCollder != null)
        {
            interactionCollder.Init(this);
        }
       
    }



}
