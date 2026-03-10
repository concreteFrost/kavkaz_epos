using UnityEngine;

public abstract class Item : MonoBehaviour, IPickable
{
    public Vector3 InitialPosition { get; set; }
    public bool IsPicked { get; set; }

    public ItemSO ItemData { get; set; }

    [SerializeField] protected InteractionCollider interactionCollder;

    public virtual void Init(ItemSO itemData)
    {
        ItemData = itemData;    

        if(interactionCollder != null)
        {
            interactionCollder.Init();
        }

        InitialPosition = transform.position;
       
    }

    public abstract void PickUp(ICollector interractor);



}
