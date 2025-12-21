public class PlayerCombatInventory : CombatInventory
{
    HumanoidCombatController combatController;
    public BareHandsWeapon bareHands;

    public override void Init(PlayerCombatInventoryServiceProvider service)
    {
        base.Init(service);

        combatController = service.combatController;
        Damagable = service.statsModifier;

        bareHands.SetOwner(this);

        DefaultWeapon = bareHands;
        CurrentWeapon = DefaultWeapon;

    }

    public override void SetWeapon(IWeapon w)
    {
       CurrentWeapon = w;
       combatController.isWeaponed = true; 
     
    }

    public override void SetShield(IShield w)
    {
        ShieldWeapon = w;        
    }

    public override void ResetWeapon()
    {

        CurrentWeapon = DefaultWeapon;
        combatController.isAttacking = false;   
        combatController.isWeaponed = false;    

    }

    public override void ResetShield()
    {

        if (ShieldWeapon == null) return;

        ShieldWeapon = null;
        combatController.isShieldRaised = false;

    }


}
