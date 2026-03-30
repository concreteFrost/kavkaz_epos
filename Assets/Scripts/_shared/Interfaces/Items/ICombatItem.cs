using UnityEngine;

public interface ICombatItem : IBreakable
{
    IInteractor Owner { get; set; }

    ItemData GetItemData();
    string InstanceID();
    void AssignToOwner(IInteractor collecter);

    void SetEquiped(bool equiped);

}