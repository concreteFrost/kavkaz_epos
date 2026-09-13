using UnityEngine;

public class DoorWithKey : Door
{
    [SerializeField] KeyItemSO doorKeySO;

    public override void OpenDoor(IInteractor interactor,float delay=0f)
    {
        if (interactor is not PlayerInteractionController) return;

        if(doorKeySO == null)
        {
            SendDoorMessage("на эту дверь не назначен ключ");
            
            return;
        }

        var playerInteractor = interactor as PlayerInteractionController;
        var keysInventory = playerInteractor.keyItemsInventory;

        var targetKey = keysInventory.HasTargetKey(doorKeySO.id);

        if(targetKey == false)
        {
            SendDoorMessage($"требуется ключ {doorKeySO.itemName}");
            PlayLockedEvent();
            return;
        }

        SendDoorMessage($"{doorKeySO.itemName} использован.");

        PlayUnlockedEvent();

        base.OpenDoor(interactor, 1.5f);
    }


}
