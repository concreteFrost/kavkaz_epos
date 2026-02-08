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
        col.enabled = false;
        anim.Animator().enabled = false;

        agentController.DisableAgent();

        foreach (var rb in rigidbodies)
        {
            rb.GetComponent<Collider>().enabled = true;
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
        }

        

        ApplyImpulseFromSource(force,from);

     
    }

    public override void DisableRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            rb.GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
            
        }

        col.enabled = true;
        anim.Animator().enabled = true;

        if (agentController.IsOnBakedArea())
            agentController.EnableAgent();

        else
            InvokeInvalidRecover();

    }


   
}

