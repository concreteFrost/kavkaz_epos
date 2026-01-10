
using UnityEngine;

public class HumanoidAnimatorIK : MonoBehaviour
{
    
    public Animator animator;
    private HumanoidAIMotor aiMotor;
    private CharacterStats stats;

    private float currLookWeight;
    private float weightVelocity;
    public float weight=2;
    public float headWeight=0.3f;
    public float bodyWeight=0.1f;

    Vector3 lastLookPos;

    public void Init(HumanoidAIMotor motor, CharacterStats stats)
    {
        this.aiMotor = motor;   
        this.stats = stats; 
    }

    private void OnAnimatorIK(int layerIndex)
    {
        bool hasTarget = aiMotor.rotateTarget != null;
        float targetWeight = hasTarget ? weight : 0f;

        currLookWeight = Mathf.SmoothDamp(
            currLookWeight,
            targetWeight,
            ref weightVelocity,
            0.2f
        );

        if (hasTarget)
        {
            lastLookPos = aiMotor.rotateTarget.position;
        }

        animator.SetLookAtWeight(currLookWeight, bodyWeight, headWeight);

        if (currLookWeight > 0.001f)
        {
            animator.SetLookAtPosition(lastLookPos);
        }
 

    }

}
