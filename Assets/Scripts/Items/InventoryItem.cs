using UnityEngine;

public class InventoryItem : Item
{
    public ItemSO itemSO;
    public override void PickUp(IAttackSource s)
    {
        interactionCollder.DisableCollider();
        gameObject.SetActive(false);
       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init(itemSO);
    }

    // Update is called once per frame
    protected override void Init( ItemSO itemData)
    {
        base.Init(itemData);    
    }
}
