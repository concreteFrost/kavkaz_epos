using UnityEngine;

public class AiRagdollController : BaseRagdollController
{

    HumanoidAgentController agentController;

    public AiRagdollController(MonoBehaviour ctx, BaseHumanoidAnimatorController anim, HumanoidAgentController agent, Transform self)
    {
        this.agentController = agent;
        base.Init(ctx,anim, self);

        DisableRagdoll();
        
    }

    public override void EnableRagdoll(Vector3 from, float force = 0)
    {
       
        agentController.DisableAgent();

        col.enabled = false;
       
        anim.Animator().enabled = false;

        if (hipsRb.isKinematic)
            UpdateJointAnchors();

        foreach (var rb in rigidbodies)
        {
            bool wasKinematic = rb.isKinematic;
            rb.GetComponent<Collider>().enabled = true;
            rb.isKinematic = false;
            rb.useGravity = true;
            if (wasKinematic)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

        }

        ApplyImpulseFromSource(force,from);

     
    }

    public override void DisableRagdoll()
    {
        DisableRagdollPhysics();

        col.enabled = true;
        anim.Animator().enabled = true;

        if (agentController.IsOnBakedArea())
            agentController.EnableAgent();

        else
            InvokeInvalidRecover();

    }


   
}

