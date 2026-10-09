using UnityEngine;

public class GenericAttackSourceBehaviour : StateMachineBehaviour
{
    IDamageColliderActivator activator;

    bool hitActive = false;
    bool wasAudioPlayer = false;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hitActive = false;
        wasAudioPlayer = false;
        activator = animator.GetComponentInParent<IDamageColliderActivator>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float t = stateInfo.normalizedTime;

        var animationInfo = activator.AnimationInfo();
        if(!wasAudioPlayer && t >= animationInfo.audioStartTime)
        {
            //play audio
            activator.PlayAttack();
            wasAudioPlayer = true;
        }

        if(!hitActive && t >= animationInfo.hitStartFrame)
        {
            //perform hit
            activator.ActivateDamageCollider();
            hitActive = true;
        }

        if(hitActive && t>= animationInfo.hitEndFrame)
        {
            //end hit
            activator.DeactivateDamageCollider();
            hitActive = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hitActive = false;
        wasAudioPlayer = false;
    }
}
