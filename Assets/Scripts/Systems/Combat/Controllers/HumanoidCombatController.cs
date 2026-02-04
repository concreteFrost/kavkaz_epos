
public class HumanoidCombatController : BaseHumanoidCombatController<HumanoidAICombatControllerServices>
{

    private IPushable pushReceiver;

    // ================= INIT =================
    public override void Init(HumanoidAICombatControllerServices service)
    {
        base.Init(service);
        this.pushReceiver = service.pushable;
        pushReceiver.PushReceived += ForceAttackCancel;
    }

    protected override void OnDisable()
    {
        damageController.DamageTaken -= ForceAttackCancel;  
        pushReceiver.PushReceived -= ForceAttackCancel; 
    }

}
