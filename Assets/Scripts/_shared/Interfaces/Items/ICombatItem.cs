using UnityEngine;

public interface ICombatItem : IBreakable
{
    ICollector Owner { get; set; }

    string InstanceID();
    void AssignToOwner(ICollector collecter);

    void SetEquiped(bool equiped);

}