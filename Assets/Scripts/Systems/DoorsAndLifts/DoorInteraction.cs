using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] Door door;

    public string InteractionName() => "Door";

    public ItemInteractionType InteractType() => ItemInteractionType.Door;
    public bool HasInteracted 
    {
        get => door.isOpened;
        set => door.isOpened = value;
    }


    public string ActionText() => !door.isOpened ? "Open" : "";

    public bool CanInteract() => !door.isOpened && !door.isLocked;

    public void Interact(IInteractor picker)
    {
        if (!CanInteract())
            return;

        door.OpenDoor(picker);
    }

}