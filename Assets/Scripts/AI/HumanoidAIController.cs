using UnityEngine;

public class HumanoidAIController : MonoBehaviour
{
    HumanoidAIMotor aiMotor;
    HumanoidAIAnimatorController aIAnimator;
    HumanoidAgentController agentController;
    HumanoidAIDamageController damageController;

    CharacterStatsController stats;
    Transform self;

    public void Init(
         HumanoidAIMotor aiMotor,
        HumanoidAIAnimatorController aIAnimator,
        HumanoidAgentController agentController,
        HumanoidAIDamageController damageController,
        HumanoidAIAnimatorController animator,
        CharacterStatsController stats,
        Transform self
        )
    {
        this.self = self;
        this.agentController = agentController;
        this.aiMotor = aiMotor;
        this.aIAnimator = aIAnimator;
        this.damageController = damageController;

        this.stats = stats;

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
            aiMotor.moveSpeed = stats.Speed.StrafeSpeed;
        }
        else if (aiMotor.IsSprinting)
        {
            aiMotor.moveSpeed = stats.Speed.RunSpeed;
        }
        else
        {
            aiMotor.moveSpeed = stats.Speed.WalkSpeed;
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
