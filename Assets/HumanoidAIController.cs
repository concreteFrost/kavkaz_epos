using UnityEngine;

public class HumanoidAIController : MonoBehaviour
{
    HumanoidAIMotor aiMotor;
    HumanoidAIAnimator aIAnimator;
    Animator animator;
    ICharacterStatsModifier statsController;
    CharacterStats stats;

    public void Init(HumanoidAIMotor motor, 
        Animator animator, 
        HumanoidAIAnimator aIAnimator,
        CharacterStats stats,
        ICharacterStatsModifier statsModifier
        )
    {
        this.aiMotor = motor;
        this.animator = animator;
        this.aIAnimator = aIAnimator;
        this.stats = stats;
        this.statsController = statsModifier;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        UpdateMotor();
        UpdateAnimator();
        ControlSpeed();
        ControlRotation();

        if(animator.applyRootMotion == false)
        {
            animator.applyRootMotion = true;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (aiMotor.rotateTarget == null) return;

        // Target position
        Vector3 lookPos = aiMotor.rotateTarget.position;

        // Вес IK: тело+голова+глаза
        animator.SetLookAtWeight(1f, 0.5f, 1f, 0.5f, 0.5f);

        // Устанавливаем точку взгляда
        animator.SetLookAtPosition(lookPos);
    }



    private void UpdateMotor()
    {
        aiMotor.UpdateMotor(4f); // присвоить нормальное значение прыжка
    }

    private void UpdateAnimator()
    {
        aiMotor.UpdateAnimatorLocomotion();
        aIAnimator.UpdateAnimatorParameters();
    }

    #region Movement and Rotation
    private void ControlSpeed()
    {

        float baseSpeed = aiMotor.IsSprinting
            ? stats.runningSpeed
            : stats.walkSpeed;


        aiMotor.moveSpeed = Mathf.Lerp(
            aiMotor.moveSpeed,
            baseSpeed,
            aiMotor.movementSmooth * Time.deltaTime
        );

        aiMotor.agent.speed = aiMotor.moveSpeed;
    }

    private void ControlRotation()
    {
        if (aiMotor.rotateTarget != null)
        {
            aiMotor.RotateToTarget(aiMotor.rotateTarget.position);
            return;
        }

        if (!aiMotor.agent.hasPath)
            return;

        Vector3 dir = aiMotor.agent.steeringTarget - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f)
            return;

        aiMotor.RotateToDirection(dir);

    }

    #endregion

    #region Target Lock

    public void SetLockTarget(Transform target)
    {
        aiMotor.rotateTarget = target;
    }

    public void ResetLockTarget()
    {
        aiMotor.rotateTarget = null;
    }

    #endregion

}
