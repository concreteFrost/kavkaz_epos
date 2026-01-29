using UnityEngine;

public class PlayerRagdollController : BaseRagdollController
{

    PlayerInput input;


    public PlayerRagdollController(MonoBehaviour ctx, Animator anim, PlayerInput input, Transform self)
    {
        this.input = input;
        base.Init(ctx, anim, self);

        EnableRagdoll(10f, null);

    }

    public override void EnableRagdoll(float force, Transform from)
    {
        col.enabled = false;
        anim.enabled = false;

        input.controls.Disable();

        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.GetComponent<Collider>().enabled = true;
        }

        ApplyImpulseFromSource(force, from);


    }

    public override void DisableRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;

            rb.GetComponent<Collider>().enabled = false;
        }

        col.enabled = true;
        anim.enabled = true;

        input.controls.Enable();    

        //if (agentController.IsOnBakedArea())
        //    agentController.EnableAgent();

        //else
        //    InvokeInvalidRecover();

    }



}

