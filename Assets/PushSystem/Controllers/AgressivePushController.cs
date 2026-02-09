using UnityEngine;

public class AgressivePushController : MonoBehaviour , IPushSource
{

    IHumanoidCombat combatController;
    BaseHumanoidAnimatorController animatorController;

    public bool IsPushing = false;

    [SerializeField] private AnimationInfoSO animationData;
    [SerializeField] private PushCollider pushCollider;

    public AnimationInfoSO AnimationData() => animationData;

    public void Init(AgressivePushControllerServices services)
    {
        this.combatController = services.combatController;
        this.animatorController = services.animatorController;

        IsPushing = false;
        
        if(pushCollider == null)
        {
            Debug.Log("no push collider assigned");
        }

        pushCollider.Init(services.attackSource.TargetsToIgnore, services.self);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TriggerPushAnimation()
    {

        if (IsPushing || combatController.IsAttacking || combatController.IsShieldRaised) return;

        animatorController.PerformPush();

        IsPushing = true;
    }

    public void PerformPush()
    {
        pushCollider.EnableCollider();
    }

    public void CancelPush()
    {
        pushCollider.DisableCollider(); 
        IsPushing = false;
    }
}
