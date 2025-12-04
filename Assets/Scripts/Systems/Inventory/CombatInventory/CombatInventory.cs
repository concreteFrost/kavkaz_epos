using System.Collections.Generic;
using UnityEngine;

public abstract class CombatInventory : MonoBehaviour , IAttackSource
{
    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform leftHand;

    public IWeapon DefaultWeapon { get; set; } = null;

    public IWeapon CurrentWeapon { get; set; } =null;
    
    public IShield ShieldWeapon { get; set; } = null;

    private string sourceId;
    public List<string> targetsToIgnore = new List<string>();
    public string SourceId()=> sourceId;

    public List<string> TargetIds { get => targetsToIgnore; set=>targetsToIgnore=value ; }

    public Transform GetRightHand() => rightHand;
    public Transform GetLeftHand() => leftHand;

    public abstract void SetWeapon(IWeapon w);

    public abstract void SetShield(IShield w);

    public abstract void ResetWeapon();

    public abstract void ResetShield();

    public virtual void Init(ICharacterAnimator anim)
    {
        sourceId = GetInstanceID().ToString(); 
    }



}
