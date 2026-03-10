using UnityEngine;


public interface IPickable
{
    Vector3 InitialPosition { get; set; }   
    public bool IsPicked { get; set; }

    public ItemSO ItemData {  get; set; }   

    void PickUp(ICollector picker);


}
