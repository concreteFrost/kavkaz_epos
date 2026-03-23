using UnityEngine;


public interface IPickable
{
    Vector3 InitialPosition { get; set; }   
    public bool HasInteracted { get; set; }
    bool CanInteract();

    void PickUp(ICollector picker);


}
