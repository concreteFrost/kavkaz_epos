
using UnityEngine;
using UnityEngine.AI;

public class HumanoidAIMotor : BaseHumanoidMotor
{
    [HideInInspector] public NavMeshAgent agent;

    public override void Init(Animator anim)
    {
        animator = anim;
        animator.updateMode = AnimatorUpdateMode.Fixed;
        animator.applyRootMotion = true;
        animationSmooth = 0.5f;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.angularSpeed = 0;
        agent.acceleration = 50f;
        agent.stoppingDistance = 0;
        agent.autoBraking = true;
    }

    public override void AirControl()
    {
        if (isGrounded) return;

        // Для AI можно просто обновлять moveDirection для анимации
        moveDirection.y = 0;
        moveDirection.x = Mathf.Clamp(moveDirection.x, -1f, 1f);
        moveDirection.z = Mathf.Clamp(moveDirection.z, -1f, 1f);

        // Можно хранить высоту для анимации прыжка
        if (transform.position.y > heightReached) heightReached = transform.position.y;

        // Движение NavMeshAgent в воздухе не контролируем через Rigidbody
    }

    public override void UpdateAnimatorLocomotion()
    {
        base.UpdateAnimatorLocomotion();
    }
  

    public override void MoveCharacter(Vector3 direction)
    {

        // направление для визуального поворота
        Vector3 desiredDir = (direction - transform.position).normalized;
        desiredDir.y = 0f;

        moveDirection = desiredDir; // только для анимации / поворота

        // реальное движение через NavMesh
        agent.SetDestination(direction);
    }

    public override void RotateToTarget(Vector3 targetPosition)
    {
        if (moveDirection.sqrMagnitude < 0.1f) return;
        base.RotateToTarget(targetPosition);    
    }

    public override void StopMovement()
    {
        moveDirection = Vector3.zero;
        inputMagnitude = 0f;
        agent.ResetPath();

    }

    public override void UpdateMotor(float jumpHeight)
    {

        CheckGround();
        AirControl();
      
    }

    public override void UseRootMotion()
    {

    }

    public override void UseRootMotionWithObstacles()
    {
        // Для AI коллизии NavMeshAgent уже работают
        transform.position += animator.deltaPosition;
        transform.rotation *= animator.deltaRotation;
    }

    protected override void CheckGround()
    {
        //CheckGroundDistance();


        // NavMeshAgent всегда на поверхности
        isGrounded = true;

        // Если нужно для анимации — можно имитировать падение/высоту
        groundDistance = 0f;
        verticalVelocity = 0f;
    }

    protected override void CheckGroundDistance()
    {
        // Для ИИ на NavMeshAgent дистанцию до земли можно считать 0
        groundDistance = 0f;
    }

    protected override void ControlJumpBehaviour(float jumpHeight)
    {
        //ожидает
    }
}
