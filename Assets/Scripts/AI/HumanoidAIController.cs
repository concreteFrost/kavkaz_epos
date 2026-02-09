using UnityEngine;

public class HumanoidAIController : MonoBehaviour
{
    HumanoidAIMotor aiMotor;
    HumanoidAIAnimatorController aIAnimator;
    HumanoidAgentController agentController;
    HumanoidAIDamageController damageController;
    HumanoidAIAnimatorController animator;
    HumanoidStats stats;


    Transform self;

    public void Init(HumanoidControllerServices services)
    {
        this.self = services.self;
        this.agentController = services.agentController; 
        this.animator = services.aiAnimatorController;
        this.aiMotor = services.aiMotor;
        this.aIAnimator = services.aiAnimatorController;
        this.damageController = services.damageController;

        this.stats = services.statsManager.Stats;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        UpdateMotor();
        UpdateAnimator();
        ControlSpeed();
        ControlRotation();
    }


    private void UpdateMotor()
    {
        aiMotor.UpdateMotor(stats.jumpHeight); // присвоить нормальное значение прыжка
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
            aiMotor.moveSpeed = 0f;        
        }

        else if (aiMotor.IsStrafing)
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

        agentController.agent.speed = aiMotor.moveSpeed;
    }

    private void ControlRotation()
    {
        if (damageController.IsDamaged) return;

        if (aiMotor.inputMagnitude == 0) return;

        if (aiMotor.rotateTarget != null)
        {
            aiMotor.RotateToTarget(aiMotor.rotateTarget.position);
            return;
        }

        Vector3 dir = agentController.agent.steeringTarget - self.position;
        dir.y = 0f;

        aiMotor.RotateToDirection(dir);

    }

    #endregion



}
