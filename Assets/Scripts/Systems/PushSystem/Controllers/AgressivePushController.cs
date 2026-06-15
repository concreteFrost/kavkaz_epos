using System.Collections;
using UnityEngine;


public class AgressivePushController : MonoBehaviour , IPushSource
{

    IHumanoidMeleeCombat combatController;
    BaseHumanoidAnimatorController animatorController;

    public bool IsPushing = false;
    [SerializeField] private bool canPush = true;

    [SerializeField] private AnimationInfoSO animationData;
    [SerializeField] private PushCollider pushCollider;

    public AnimationInfoSO AnimationData() => animationData;

    public void Init(IAttackSource attackSource, IHumanoidMeleeCombat combatController, BaseHumanoidAnimatorController animatorController)
    {
        
        this.combatController = combatController;
        this.animatorController = animatorController;

        IsPushing = false;
        
        if(pushCollider == null)
        {
            Debug.Log("no push collider assigned");
        }

        pushCollider.Init(attackSource.TargetsToIgnore,attackSource);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TriggerPushAnimation()
    {

        if (!canPush || combatController.IsAttacking || combatController.IsShieldRaised) return;

        IsPushing = true;

        animatorController.PerformPush();
        StartCoroutine(PushCooldownCoroutine());

        
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

    IEnumerator PushCooldownCoroutine()
    {
        canPush = false;
        yield return new WaitForSeconds(1f);
        canPush = true;
    }
}
