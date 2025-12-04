using System.Collections.Generic;
using UnityEngine;

public interface IAttackSource
{
    public string SourceId();

    public List<string> TargetIds { get; set; }

    public IWeapon DefaultWeapon { get; set; }

    public IWeapon CurrentWeapon { get; set; }

    public IShield ShieldWeapon  { get; set; }

    public Transform GetRightHand();
    public Transform GetLeftHand();

    public abstract void SetWeapon(IWeapon w);

    public abstract void SetShield(IShield w);

    public abstract void ResetWeapon();

    public abstract void ResetShield();
}
