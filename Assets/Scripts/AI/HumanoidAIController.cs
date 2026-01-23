using UnityEngine;

public class HumanoidAIController : MonoBehaviour
{
    HumanoidAIMotor aiMotor;
    HumanoidAIAnimatorController aIAnimator;
    IDamagable damageController;
    Animator animator;
    //ICharacterStatsController statsController;
    HumanoidStats stats;

    public void Init(HumanoidControllerService service)
    {
        this.animator = service.animator;
        this.aiMotor = service.aiMotor;
        this.aIAnimator = service.aiAnimatorController;
        //this.statsController = service.statsController;
        this.damageController = service.damageController;
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

        //if (animator.applyRootMotion == false)
        //{
        //    animator.applyRootMotion = true;
        //}
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
        

        if (aiMotor.StopMove || aiMotor.IsDodging || damageController.IsDamaged)
        {
            aiMotor.agent.speed = 0f;
            return;
        }


        if (aiMotor.IsStrafing)
        {
            aiMotor.moveSpeed = stats.strafeSpeed;
        }
        else if (aiMotor.IsSprinting)
        {
            aiMotor.moveSpeed = stats.runningSpeed;
        }
        else
        {
            aiMotor.moveSpeed = stats.walkSpeed;
        }

        aiMotor.agent.speed = aiMotor.moveSpeed;
    }

    private void ControlRotation()
    {
        if (damageController.IsDamaged) return;

        if (aiMotor.inputMagnitude == 0) return;

        if (!aiMotor.isGrounded) return;

       
        if (aiMotor.rotateTarget != null)
        {
            aiMotor.RotateToTarget(aiMotor.rotateTarget.position);
            return;
        }


        Vector3 dir = aiMotor.agent.steeringTarget - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f)
            return;

        aiMotor.RotateToDirection(dir);

    }

    #endregion



}
