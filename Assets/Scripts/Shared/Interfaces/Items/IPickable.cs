using UnityEngine;

public enum PickableType
{
    inventoryItem = 0,
    weapon = 1,
    shield = 2,
}
public interface IPickable
{
    public bool IsPicked { get; set; }

    public ItemSO ItemData {  get; set; }   

    public abstract void PickUp(IAttackSource attackSource);
}
