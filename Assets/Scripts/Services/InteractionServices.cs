
public class HumanoidInteractService
{
    public ICombatInventory combatInventory;
    public IDamagable owner;
    public IAttackSource attackSource;

    public HumanoidInteractService(ICombatInventory combatInventory, IDamagable owner, IAttackSource attackSource)
    {
        this.combatInventory = combatInventory;
        this.owner = owner;
        this.attackSource = attackSource;
    }
}
