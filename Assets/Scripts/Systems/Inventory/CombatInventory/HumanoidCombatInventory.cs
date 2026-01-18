using UnityEngine;

public class HumanoidCombatInventory : BaseCombatInventory
{
    [Header("Bare Hands Settings")]

    private BareHandsWeapon bareHands;
    [SerializeField] private WeaponSO barehandsData;
    [SerializeField] private WeaponDamageCollider barehandDamageCollider;
    

    public override void Init(HumanoidCombatInventoryService service)
    {
        base.Init(service);

        //targetsToIgnore.Add(CharacterType.Player);
        //targetsToIgnore.Add(CharacterType.FriendlyNPC);

        InitializeBarehands();  

        DefaultWeapon = bareHands;
        CurrentWeapon = DefaultWeapon;

    }

    private void InitializeBarehands()
    {

        bareHands = new BareHandsWeapon();
        bareHands.Init(barehandsData, barehandDamageCollider, this);

    }

    public override void SetWeapon(IWeapon w)
    {
       CurrentWeapon = w;
       combatController.IsWeaponed = true; 
     
    }

    public override void SetShield(IShield w)
    {
        ShieldWeapon = w;        
    }

    public override void ResetWeapon()
    {

        CurrentWeapon = DefaultWeapon;  
        combatController.IsWeaponed = false;    

    }

    public override void ResetShield()
    {

        if (ShieldWeapon == null) return;

        ShieldWeapon = null;
        combatController.IsShieldRaised = false;

    }


}
