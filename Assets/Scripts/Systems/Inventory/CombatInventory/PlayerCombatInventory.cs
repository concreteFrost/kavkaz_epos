public class PlayerCombatInventory : BaseCombatInventory
{
    HumanoidCombatController combatController;
    public BareHandsWeapon bareHands;

    public override void Init(PlayerCombatInventoryServiceProvider service)
    {
        base.Init(service);

        combatController = service.combatController;

        bareHands.SetOwner(this);

        DefaultWeapon = bareHands;
        CurrentWeapon = DefaultWeapon;

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
        combatController.IsAttacking = false;   
        combatController.IsWeaponed = false;    

    }

    public override void ResetShield()
    {

        if (ShieldWeapon == null) return;

        ShieldWeapon = null;
        combatController.IsShieldRaised = false;

    }


}
