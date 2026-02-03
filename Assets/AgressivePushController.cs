using UnityEngine;

public class AgressivePushController : MonoBehaviour , IPushSource
{
    IAttackSource attackSource;
    IHumanoidCombat combatController;
    BaseHumanoidAnimatorController animatorController;

    [SerializeField] private AnimationInfoSO animationData;
    [SerializeField] private PushCollider pushCollider;

    public bool IsPushing { get; set; }
    public AnimationInfoSO AnimationData() => animationData;

    public void Init(IAttackSource attackSource, IHumanoidCombat combatController, BaseHumanoidAnimatorController animatorController)
    {
        this.combatController = combatController;
        this.animatorController = animatorController;

        IsPushing = false;
        
        if(pushCollider == null)
        {
            Debug.Log("no push collider assigned");
        }

        pushCollider.Init(attackSource.TargetsToIgnore);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TriggerPushAnimation()
    {
        if (combatController.IsAttacking)
        {
            CancelPush();
            return;
        }
        if (IsPushing) return;

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
