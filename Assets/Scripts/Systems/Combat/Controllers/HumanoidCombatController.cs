
public class HumanoidCombatController : BaseHumanoidCombatController<HumanoidAICombatControllerServices>
{

    private IPushable pushReceiver;

    // ================= INIT =================
    public override void Init(HumanoidAICombatControllerServices service)
    {
        base.Init(service);
      
    }

    //private void Update()
    //{
    //    if(pushReceiver == null) return; 

    //    if (pushReceiver.IsPushed) // костыль для отмены удара во время пинка
    //        ForceAttackCancel(null);
    //}
}
