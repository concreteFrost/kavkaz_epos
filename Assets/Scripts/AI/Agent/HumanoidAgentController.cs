using UnityEngine;
using UnityEngine.AI;


public class HumanoidAgentController
{
    public NavMeshAgent agent;

    public HumanoidAgentController(NavMeshAgent agent, Animator animator)
    {
        this.agent = agent;

        agent.updatePosition = true;
        agent.updateRotation = false; // поворот контролируешь сам

        agent.angularSpeed = 0f;       // чтобы агент не крутил
        agent.acceleration = 80f;      // отзывчивость
        agent.stoppingDistance = 0.8f;
        agent.autoBraking = true;

        agent.obstacleAvoidanceType =
            ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }

    #region Agent Control

    public void EnableAgent()
    {
        agent.enabled = true;
        agent.ResetPath();
       
    }

    public void DisableAgent()
    {
        if (!agent.isActiveAndEnabled) return;

        agent.ResetPath();
        agent.enabled = false;
    }
    public void StopAgent()
    {
        if (!agent.isActiveAndEnabled) return;
        
        agent.ResetPath();

    }

    public void SendAgentToPosition(Vector3 dir)
    {
        if (!agent.isActiveAndEnabled) return;

        agent.SetDestination(dir);
    }

    public void MoveAgentToPosition(Vector3 dir)
    {
        if (!agent.isActiveAndEnabled) return;

        agent.Move(dir);
    }

    public void ResetAgent()
    {
        if (!agent.isActiveAndEnabled) return;

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

    public void SetStartJump()
    {
        agent.updatePosition = false;
        //agent.updateRotation = false;
    }

    public void FinishJump(Vector3 destination)
    {
        agent.Warp(destination);
        agent.CompleteOffMeshLink();

        agent.updatePosition = true;
    }



    #endregion
}
