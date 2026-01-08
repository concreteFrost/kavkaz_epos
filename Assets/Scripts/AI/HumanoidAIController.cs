using UnityEngine;

public class HumanoidAIController : MonoBehaviour
{
    HumanoidAIMotor aiMotor;
    HumanoidAIAnimatorController aIAnimator;
    Animator animator;
    //ICharacterStatsController statsController;
    CharacterStats stats;

    public void Init(HumanoidControllerService service)
    {
        this.animator = service.animator;
        this.aiMotor = service.aiMotor;
        this.aIAnimator = service.aiAnimatorController;
        //this.statsController = service.statsController;
        this.stats = service.stats; 
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

        //float baseSpeed = aiMotor.IsSprinting
        //    ? stats.runningSpeed
        //    : stats.walkSpeed;


        //aiMotor.moveSpeed = Mathf.Lerp(
        //    aiMotor.moveSpeed,
        //    baseSpeed,
        //    aiMotor.movementSmooth * Time.deltaTime
        //);

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



}
