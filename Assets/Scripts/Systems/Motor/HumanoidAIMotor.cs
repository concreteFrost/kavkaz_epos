
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class HumanoidAIMotor : BaseHumanoidMotor
{

    HumanoidAgentController agentController;
    public Coroutine strafeCoroutine;

    public LayerMask wallLayer = 1 << 8;

    public void Init(BaseHumanoidAnimatorController anim, HumanoidAgentController agentController)
    {
        this.animator = anim;
        this.agentController = agentController;

        // Animator
        animator.Animator().updateMode = AnimatorUpdateMode.Normal;
        animator.Animator().applyRootMotion = false;
        animationSmooth = 0.2f;

        // NavMeshAgent

        isGrounded = true;

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

        //Debug.Log("rotating to target");
    }

    public override void RotateToDirection(Vector3 direction)
    {
        base.RotateToDirection(direction);
    }


    #endregion

    #region Agent Control
    public override void StopMovement()
    {

        agentController.StopAgent();
        moveDirection = Vector3.zero;
        inputMagnitude = 0f;


    }

    public void ResetSprint()
    {
        isSprinting = false;
    }

    #endregion

    #region Motor Control
    public override void UpdateMotor(float jumpHeight)
    {

        CheckGround();
        AirControl();
        ControlJumpBehaviour(jumpHeight);

    }

    public override void MoveCharacter(Vector3 direction)
    {
        // направление для визуального поворота
        Vector3 desiredDir = (direction - transform.position).normalized;
        desiredDir.y = 0f;

        moveDirection = desiredDir; // только для анимации 
        
        agentController.SendAgentToPosition(direction);
    }

    public void MoveLocal(Vector3 direction)
    {

        Vector3 velocity =
            direction.normalized * agentController.agent.speed * Time.deltaTime;

        agentController.MoveAgentToPosition(velocity);

        moveDirection = direction.normalized; // для анимации
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

    public override void SetStrafe(bool isStrafing)
    {
        base.isStrafing = isStrafing;
    }
    #endregion

    #region Strafe Control

    public void StartStrafe(Transform self, Transform target)
    {
        strafeCoroutine = StartCoroutine(StrafeCoroutine(self, target));
    }

    private IEnumerator StrafeCoroutine(Transform self, Transform target)
    {
        bool isRight = Random.value > 0.5f;
        float elapsed = 0f;
        const float maxStrafeTime = 3f;

        while (elapsed < maxStrafeTime && target != null)
        {
            Vector3 toTarget = (target.position - self.position).normalized;
            Vector3 strafeDir = Vector3.Cross(Vector3.up, toTarget).normalized;

            MoveLocal(isRight ? strafeDir : -strafeDir);

            elapsed += Time.deltaTime;
            yield return null;
        }

        strafeCoroutine = null; // флаг окончания
    }

    public void StopStrafe()
    {
        if(strafeCoroutine != null)
        {
            StopCoroutine(strafeCoroutine); 
            strafeCoroutine = null; 
        }
    }
    #endregion

    #region Root Motion Control
    public override void UseRootMotion()
    {

    }

    public override void UseRootMotionWithObstacles()
    {
        
    }

   

    #endregion

    #region Ground Control
    protected override void CheckGround()
    {
        if (isJumping)
        {
            isGrounded = false;
            return;
        }

        isGrounded = true;
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

    IEnumerator TraverseOffMeshLink(OffMeshLinkData data, float height)
    {
        agentController.SetStartJump();

        isJumping = true;
        isGrounded = false;

        Vector3 start = transform.position;
        Vector3 end = data.endPos;

        yield return JumpParabola(start, end, 1.2f);

        agentController.FinishJump(end);

        isJumping = false;
        isGrounded = true;
    }

    IEnumerator JumpParabola(Vector3 start, Vector3 end, float height)
    {
        float t = 0f;

        float distance = Vector3.Distance(start, end);
        float horizontalSpeed = 1f; // м/с — подбирается визуально

        float duration = distance / horizontalSpeed;
        duration = Mathf.Clamp(duration, 0.35f, 1.2f);

        //animator.CrossFadeInFixedTime("Jump", 0.1f);


        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            float yOffset = 4f * height * t * (1 - t);
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += yOffset;

            transform.position = pos;

            yield return null;
        }

        //transform.position = end;
    }


    protected override void ControlJumpBehaviour(float jumpHeight)
    {
        if (agentController.agent.isOnOffMeshLink && isGrounded)
        {
            StartCoroutine(TraverseOffMeshLink(agentController.agent.currentOffMeshLinkData, jumpHeight));
            base.Jump(jumpHeight);
        }

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
