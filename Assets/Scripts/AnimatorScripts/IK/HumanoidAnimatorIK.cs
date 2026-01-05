using UnityEngine;

public class HumanoidAnimatorIK : MonoBehaviour
{
    public HumanoidAIMotor aiMotor;
    public Animator animator;

    private float currLookWeight;
    private float weightVelocity;
    public float weight=2;
    public float headWeight=0.3f;
    public float bodyWeight=0.1f;
    

    private void OnAnimatorIK(int layerIndex)
    {
        float target = aiMotor.rotateTarget != null ? weight : 0;
       
        currLookWeight = Mathf.SmoothDamp(currLookWeight, target, ref weightVelocity, 0.1f);
        
        // Target position
        Vector3 lookPos = aiMotor.rotateTarget != null ? aiMotor.rotateTarget.position : Vector3.zero;

        // Вес IK: тело+голова+глаза
        animator.SetLookAtWeight(currLookWeight,bodyWeight,headWeight);

        // Устанавливаем точку взгляда
        animator.SetLookAtPosition(lookPos);
    }

}
