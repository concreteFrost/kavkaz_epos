using UnityEngine;

public class HumanoidPushReceiver : PushReceiver
{
    IDamagable damageController;
    BaseHumanoidAnimatorController animatorController;
    HumanoidAgentController agentController;
    IRagdollController ragdollController;

    Vector3 aimingSpot;

    public LayerMask groundLayer = 1 << 0;

    public void Init(IDamagable damageController, BaseHumanoidAnimatorController animatorController, HumanoidAgentController agentController, IRagdollController ragdollController, Transform self)
    {
        this.damageController = damageController;
        this.animatorController = animatorController;
        this.agentController = agentController;
        this.ragdollController = ragdollController; 
        this.self = self;

        characterType = damageController.CharacterType;
    }

    public override void CancelPush()
    {
        if (damageController.IsDead) return;

        //agentController.ToggleUpdatePosition(false);

        IsPushed = false;   
    }

    public override void GetPushed(PushDirection dir, Vector3 aimingSpot)
    {
        if (IsPushed) return;

        //agentController.ToggleUpdatePosition(false);
        animatorController.GetPushed(dir);

        this.aimingSpot = aimingSpot;   

        IsPushed = true;
    }

    public override void TrackPush()
    {
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.position = aimingSpot;
        if (!IsPushed) return;

        Vector3 origin = aimingSpot;


        float dist = Vector3.Distance(self.transform.position, aimingSpot);



        //if(dist< 0.2f)
        if (!Physics.SphereCast(
                origin,
                0.2f,
                Vector3.down,
                out RaycastHit hit,
                2f,
                groundLayer))
        {
            ragdollController.Knockout(0, null);
            IsPushed = false;
        }
    }


}
