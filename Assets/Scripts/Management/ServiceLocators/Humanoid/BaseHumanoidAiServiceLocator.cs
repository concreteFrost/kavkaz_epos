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
    [SerializeField] protected CharacterStatsController statsManager;
    [SerializeField] protected CharacterStatsModifier statsModifier;


    [Header("Система урона")]
    [SerializeField] protected HumanoidAIDamageController damageController;
    [SerializeField] protected HumanoidAIPushReceiver pushReceiver;
    [SerializeField] protected AiFallController fallController;

    [Header("Визуальные эффекты")]
    [SerializeField] protected CharacterEffectVisualizer visualizer;

    [Header("Жизненый цикл")]
    [SerializeField] protected HumanoidAiLifecycle lifecycle;


    protected string uid;
    protected AiRagdollController ragdollController;
    protected HumanoidAIAnimatorController animatorController;
    protected HumanoidAgentController agentController;

    private void Awake()
    {
        uid = uniqueId.uniqueId;
        CoreInit();
        
    }

    protected virtual void CoreInit()
    {
        AnimatorInit();
        AgentInit();
        RagdollInit();
        IKInit();
        MotorInit();
        ControllerInit();
        StatsInit();
        DamageInit();
        LifecycleInit();
    }

    private void IKInit()
    {
        ik.Init(motor: motor, damageController: damageController);
    }

    private void AgentInit()
    {
        agentController = new HumanoidAgentController(agent: agent, animator: animator);
    }

    private void RagdollInit()
    {
        ragdollController = new AiRagdollController(ctx: this, anim: animatorController, agent: agentController, self: transform);
    }

    private void StatsInit()
    {
        statsManager.Init();
        statsModifier.Init(statsManager,visualizer);
    }

    private void ControllerInit()
    {
        controller.Init(aiMotor: motor, aIAnimator: animatorController, agentController: agentController, animator: animatorController, damageController: damageController, stats: statsManager, self: transform);
    }

    private void MotorInit()
    {
        motor.Init(anim: animatorController, agentController: agentController);
    }

    private void DamageInit()
    {
        damageController.Init(self: transform, motor: motor, statsController: statsManager, ragdollController: ragdollController, animatorController: animatorController, statsModifier:statsModifier);
        pushReceiver.Init(damageController: damageController, animatorController: animatorController, ragdollController: ragdollController, self: transform);
        fallController.Init(ragdollController: ragdollController, damagable: damageController, self: transform);
    }

    protected abstract void LifecycleInit();
  


    protected abstract void AnimatorInit();
   






}
