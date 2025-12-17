public class PlayerCombatInventory : CombatInventory
{
    PlayerMotor motor;

    public BareHandsWeapon bareHands;

    public override void Init(PlayerCombatInventoryServiceProvider service)
    {
        base.Init(service);

        motor = service.motor;
        Damagable = service.statsModifier;

        bareHands.SetOwner(this);

        DefaultWeapon = bareHands;
        CurrentWeapon = DefaultWeapon;

    }

    public override void SetWeapon(IWeapon w)
    {
       CurrentWeapon = w;
       motor.IsWeaponed = true; 
     
    }

    public override void SetShield(IShield w)
    {
        ShieldWeapon = w;        
    }

    public override void ResetWeapon()
    {

        CurrentWeapon = DefaultWeapon;
        motor.IsAttacking = false;   
        motor.IsWeaponed = false;    

    }

    public override void ResetShield()
    {

        if (ShieldWeapon == null) return;

        ShieldWeapon = null;
        motor.IsShieldRaised = false;

    }


}
