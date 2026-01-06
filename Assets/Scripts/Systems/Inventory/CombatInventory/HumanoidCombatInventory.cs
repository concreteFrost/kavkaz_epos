public class HumanoidCombatInventory : BaseCombatInventory
{
    IHumanoidCombat combatController;
    public BareHandsWeapon bareHands;

    public override void Init(HumanoidCombatInventoryService service)
    {
        base.Init(service);

        combatController = service.combatController;

        bareHands.SetOwner(this);

        DefaultWeapon = bareHands;
        CurrentWeapon = DefaultWeapon;

        //targetsToIgnore.Add(CharacterType.Player);
        //targetsToIgnore.Add(CharacterType.FriendlyNPC);

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
