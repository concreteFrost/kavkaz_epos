using UnityEngine;


public interface IPickable
{
    public bool IsPicked { get; set; }

    public ItemSO ItemData {  get; set; }   

    void PickUp(ICollector picker);


}
