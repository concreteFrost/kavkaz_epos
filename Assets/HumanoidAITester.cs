using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidAITester : MonoBehaviour
{
    public HumanoidAIMotor aiMotor;
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

        float distance = Vector3.Distance(defaultPosition, transform.position);

        if (distance < 1f)
        {
            aiMotor.StopMovement();
            isMovingToDefault = false;
            
        }
       
    }

}
