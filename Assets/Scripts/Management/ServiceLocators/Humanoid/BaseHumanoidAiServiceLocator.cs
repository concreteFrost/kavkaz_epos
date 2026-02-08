using UnityEngine;
using UnityEngine.AI;

public abstract class BaseHumanoidAiServiceLocator : MonoBehaviour
{
    [Header("Уникальный идентификатор")]
    [SerializeField] protected UniqueId uniqueId;
   
    [Header("Анимация")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimatorOverrideController overrideController;
    [SerializeField] protected HumanoidAnimatorIK ik;
   
    [Header("Агент")]
    [SerializeField] protected NavMeshAgent agent;
   
    [Header("Мотор")]
    [SerializeField] protected HumanoidAIMotor motor;
    [SerializeField] protected HumanoidAIController controller;

    [Header("Статы")]
    [SerializeField] protected CharacterStatsController statsController;
    [SerializeField] protected HumanoidStats stats;

    [Header("Система урона")]
    [SerializeField] protected HumanoidAIDamageController damageController;
    [SerializeField] protected HumanoidAIPushReceiver pushReceiver;
    [SerializeField] protected AiFallController fallController;

    [Header("Коллайдер")]
    [SerializeField] protected CapsuleCollider capsuleCollider;

    protected string uid;
    protected AiRagdollController ragdollController;
    protected HumanoidAIAnimatorController animatorController = new HumanoidAIAnimatorController();
    protected HumanoidAgentController agentController;

    private void Awake()
    {
        uid = uniqueId.uniqueId;

        CoreInit();
        BrainInit();
    }

    protected virtual void CoreInit()
    {
        AnimatorInit();
        AgentInit();
        IKInit();
        RagdollInit();
        ControllerInit();
        StatsInit();
        DamageInit();

    }

    protected abstract void AnimatorInit();

    protected abstract void BrainInit();

    protected virtual void RagdollInit()
    {
        ragdollController = new AiRagdollController(this, animatorController, agentController, transform);
    }

    protected virtual void StatsInit()
    {
        stats.Init();

        HumanoidStatsControllerServices service = new HumanoidStatsControllerServices(stats);
        statsController.Init(service);

    }

    protected virtual void ControllerInit()
    {
        motor.Init(animatorController, agentController);

        HumanoidControllerServices controllerService = new HumanoidControllerServices(transform,motor, animatorController, agentController, statsController, damageController, stats);
        controller.Init(controllerService);
    }

    protected virtual void AgentInit()
    {
        agentController = new HumanoidAgentController(agent, animator);
    }

    protected virtual void IKInit()
    {
        ik.Init(motor, stats, damageController);
    }

    protected virtual void DamageInit()
    {
        HumanoidDamageServices damageService = new HumanoidDamageServices(animatorController, ragdollController, motor, statsController, stats, agentController, capsuleCollider, uid);
        damageController.Init(damageService);

        HumanoidPushServices pushServices = new HumanoidPushServices(transform, motor, animatorController, damageController, ragdollController);
        pushReceiver.Init(pushServices);

        fallController.Init(ragdollController, damageController, transform);
    }



}
