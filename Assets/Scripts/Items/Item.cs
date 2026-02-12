using UnityEngine;

public abstract class Item : MonoBehaviour
{

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
       
    }





}
