using UnityEngine;


public enum ItemInteractionType
{
    Item = 0,
    Chest = 1,
    Door = 2,
    NPC = 3,
}

public interface IInteractable
{
    ItemInteractionType InteractType();
    Vector3 InitialPosition { get; set; }  
    Vector3 InitialRotation { get; set; }
    public bool HasInteracted { get; set; }
    bool CanInteract();

    void Interact(IInteractor picker);

}

