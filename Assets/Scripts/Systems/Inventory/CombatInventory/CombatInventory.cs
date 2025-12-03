using UnityEngine;

public abstract class CombatInventory : MonoBehaviour
{
    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform leftHand;

    public IWeapon defaultWeapon;
    public IWeapon currentWeapon;
    
    public IShield shieldWeapon = null;

    public Transform GetRightHand() => rightHand;
    public Transform GetLeftHand() => leftHand;

    public abstract void SetWeapon(IWeapon w);

    public abstract void SetShield(IShield w);

    public abstract void ResetWeapon();

    public abstract void ResetShield();

}
