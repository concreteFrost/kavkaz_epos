using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Windows;

public class HumanoidAIController : MonoBehaviour
{
    HumanoidAIMotor aiMotor;
    HumanoidAIAnimator aIAnimator;
    Animator animator;

    Vector3 defaultPosition;

    public void Init(HumanoidAIMotor motor, Animator animator, HumanoidAIAnimator aIAnimator)
    {
        this.aiMotor = motor;
        this.animator = animator;
        this.aIAnimator = aIAnimator;

        defaultPosition = transform.position;
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

    private void OnAnimatorMove()
    {
        aiMotor.UseRootMotion();

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

    //Тестовые функции
    private void ControlSpeed()
    {


        float baseSpeed = aiMotor.IsSprinting
            ? 7
            : 3;


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

}
