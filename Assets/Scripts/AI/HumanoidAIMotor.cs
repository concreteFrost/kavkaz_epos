
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
        animationSmooth = 0.25f;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.angularSpeed = 0;
        agent.acceleration = 50f;
        agent.stoppingDistance = 1f;
        agent.autoBraking = true;
    }

  

    public override void UpdateAnimatorLocomotion()
    {
        base.UpdateAnimatorLocomotion();
    }

    #region Target Lock Control
    public void SetLockTarget(Transform target)
    {
        rotateTarget = target;
        
    }

    public void ResetLockTarget()
    {
        rotateTarget = null;
       
    }
    #endregion


    #region Rotation Control
    public override void RotateToTarget(Vector3 targetPosition)
    {
        //if (moveDirection.sqrMagnitude < 0.1f) return;
        base.RotateToTarget(targetPosition);    
    }

    public override void RotateToDirection(Vector3 direction)
    {
        base.RotateToDirection(direction);
    }

    #endregion

    #region Agent Control
    public override void StopMovement()
    {
        if (!agent.isActiveAndEnabled) return;

        moveDirection = Vector3.zero;
        inputMagnitude = 0f;
        agent.ResetPath();

    }

    public void ResetSpeed()
    {
        moveDirection = Vector3.zero;
        inputMagnitude = 0f;
       
    }

    public void ResetSprint()
    {
        isSprinting = false;
    }

    public void ResetPath()
    {
        agent.ResetPath();
    }

    public bool HasReachedDestination(float tolerance = 0.1f)
    {
        if (!agent.isActiveAndEnabled || !agent.hasPath)
            return true; // если агент не активен или путь пустой, считаем, что достиг

        // оставшееся расстояние
        float remaining = agent.remainingDistance;

        // NavMeshAgent может выдавать Infinity в некоторых случаях, проверяем
        if (float.IsInfinity(remaining))
            return false;

        // считаем, что достиг, если осталось меньше stoppingDistance + tolerance
        return remaining <= agent.stoppingDistance + tolerance;
    }

    #endregion

    #region Motor Control
    public override void UpdateMotor(float jumpHeight)
    {

        CheckGround();
        AirControl();
      
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

    public override void Dodge(Vector2 dir)
    {
        isDodging = true;

        float _dodgeX = 0f;
        float _dodgeY = 0f;

        if (dir.sqrMagnitude < 0.01f)
        {
            _dodgeY = -1f;
        }
        else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            _dodgeX = Mathf.Sign(dir.x);
        }
        else
        {
            _dodgeY = Mathf.Sign(dir.y);
        }

        dodgeX = _dodgeX;
        dodgeY = _dodgeY;
    }
    #endregion

    #region Root Motion Control
    public override void UseRootMotion()
    {

    }

    public override void UseRootMotionWithObstacles()
    {
        // Для AI коллизии NavMeshAgent уже работают
        transform.position += animator.deltaPosition;
        transform.rotation *= animator.deltaRotation;
    }

    #endregion

    #region Ground Control
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

    #endregion

    #region Jump Control
    protected override void ControlJumpBehaviour(float jumpHeight)
    {
        //ожидает
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
    #endregion



}
