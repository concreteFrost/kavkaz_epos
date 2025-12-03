using UnityEngine;

public abstract class Item : MonoBehaviour, IPickable 
{

    [SerializeField] private PickableType pickableType;
    public PickableType Type() => pickableType;
    protected bool isPicked { get; set; }
    public bool IsPicked() => isPicked;

    public abstract void PickUp(Transform parent);

    public GameObject ObjectInstance {  get; set; } 

    public ItemSO ItemData { get; set; }

    protected GameObject physicalInstance;

    [SerializeField] protected InteractionCollider interactionCollder;


    protected virtual void Init(GameObject instance, ItemSO itemData)
    {
        ObjectInstance = instance;
        ItemData = itemData;    

        if(interactionCollder != null)
        {
            interactionCollder.Init(this);
        }
       
    }



}
