using UnityEngine;

public abstract class CombatInventory : MonoBehaviour
{
    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform leftHand;

    public IWeapon defaultWeapon;
    public IWeapon currentWeapon;
    
    public IWeapon shieldWeapon = null;

    public Transform GetRightHand() => rightHand;
    public Transform GetLeftHand() => leftHand;

    public abstract void SetWeapon(IWeapon w);

    public abstract void SetShield(IWeapon w);

    public abstract void ResetWeapon();

    public abstract void ResetShield();

}
