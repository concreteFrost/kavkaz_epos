using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] Door door;

    [SerializeField] bool isInteractableSide = true;

    public string InteractionName() => door.doorName;

    public ItemInteractionType InteractType() => ItemInteractionType.Door;
    public bool HasInteracted 
    {
        get => door.isOpened;
        set => door.isOpened = value;
    }


    public string ActionText() => !door.isOpened ? "Открыть" : "";

    public bool CanInteract() => !door.isOpened;

    public void Interact(IInteractor picker)
    {
      
        if (!CanInteract())
            return;

        if (!isInteractableSide)
        {
            door.SendDoorMessage("c этой стороны не открыть");
            door.PlayLockedEvent();
            return;
        }

        door.OpenDoor(picker);
    }

    private void OnDrawGizmos()
    {
        Color isInteractable = isInteractableSide ? Color.green : Color.red;
        GizmoDrawer.DrawWithCube(isInteractable, transform, transform.localScale);
    }
}

