using UnityEngine;
using UnityEngine.AI;

public class HumanoidAITester : MonoBehaviour
{
    public HumanoidAIMotor aiMotor;
    public HumanoidAIController controller; 
    public HumanoidAITargetLock targetLock;
    public Transform targetPoint;

    private bool isMovingToTarget;
    private bool isMovingToDefault;

    Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = transform.position;   
    }


    void Update()
    {
        if (aiMotor == null) return;
        if (!aiMotor.agent.enabled) return; 

        if (isMovingToTarget) MoveToTarget();
        if (isMovingToDefault) MoveToDefaultPosition();
        

    }


    public void MoveToTarget()
    {
        isMovingToTarget = true;
        isMovingToDefault = false;

        if (NavMesh.SamplePosition(targetPoint.position, out var hit, 1f, NavMesh.AllAreas))
        {
            aiMotor.MoveCharacter(hit.position);
            float distance = Vector3.Distance(hit.position, transform.position);

            if (distance < 1f)
            {
                aiMotor.StopMovement();
                isMovingToTarget = false;
            }
        }
        else
        {
            Debug.Log("path is invalid");
            aiMotor.StopMovement();
            isMovingToTarget = false;
        }
    }


    public void MoveToDefaultPosition()
    {
        isMovingToDefault=true;
        isMovingToTarget=false;

        aiMotor.MoveCharacter(defaultPosition);
        controller.ResetLockTarget();

        float distance = Vector3.Distance(defaultPosition, transform.position);

        if (distance < 1f)
        {
            aiMotor.StopMovement();
            isMovingToDefault = false;
            
        }
       
    }

    public void SetLockOnTarget()
    {
        //var t =  targetLock.CheckNearestTarget();

        //Debug.Log(t);
        //if(t != null) 
        //controller.SetLockTarget(t);

    }

    public void ResetLockTarget()
    {
       controller.ResetLockTarget();  
    }

    public void Dodge()
    {
        if (!targetLock.IsLockedOnTarget) return;

        aiMotor.agent.ResetPath();

        // направление ќ“ цели
        Vector3 fromTarget = (transform.position - targetPoint.position).normalized;

        //// локальные направлени€
        //Vector3 back = fromTarget;
        //Vector3 left = Quaternion.AngleAxis(-90f, Vector3.up) * fromTarget;
        //Vector3 right = Quaternion.AngleAxis(90f, Vector3.up) * fromTarget;

        //int choice = Random.Range(0, 3);

        //Vector3 chosenDir = choice switch
        //{
        //    0 => back,
        //    1 => left,
        //    _ => right
        //};

        //// переводим в локальное пространство персонажа
        //Vector3 localDir = transform.InverseTransformDirection(chosenDir);

        //Vector2 dodgeInput = new Vector2(localDir.x, localDir.z).normalized;

        aiMotor.Dodge(fromTarget);

    }

}
