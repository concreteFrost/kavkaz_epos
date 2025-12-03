using UnityEngine;

public enum PickableType
{
    inventoryItem = 0,
    weapon = 1,
    shield = 2,
}
public interface IPickable
{
    public PickableType Type();
    public bool IsPicked();
    public abstract void PickUp(Transform parent);
  
    public ItemSO ItemData {  get; set; }   

    public GameObject ObjectInstance { get; set; }
}
