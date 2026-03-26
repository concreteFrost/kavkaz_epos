using UnityEngine;

public interface ICombatItem : IBreakable
{

    ICollector Owner { get; set; }

    void AssignToOwner(ICollector collecter);
    //void Drop();

}