using UnityEngine;

public class InventoryItem : Item
{
    public ItemSO itemSO;
    public override void PickUp(Transform parent)
    {
        interactionCollder.DisableCollider();
        gameObject.SetActive(false);
       
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init(this.gameObject, itemSO);
    }

    // Update is called once per frame
    protected override void Init(GameObject instance, ItemSO itemData)
    {
        base.Init(instance, itemData);    
    }
}
