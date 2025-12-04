using UnityEngine;

public abstract class Item : MonoBehaviour, IPickable 
{

    public bool IsPicked { get; set; }

    public abstract void PickUp(IAttackSource s);

    public ItemSO ItemData { get; set; }

    protected GameObject physicalInstance;

    [SerializeField] protected InteractionCollider interactionCollder;


    protected virtual void Init(ItemSO itemData)
    {
        ItemData = itemData;    

        if(interactionCollder != null)
        {
            interactionCollder.Init(this);
        }
       
    }



}
