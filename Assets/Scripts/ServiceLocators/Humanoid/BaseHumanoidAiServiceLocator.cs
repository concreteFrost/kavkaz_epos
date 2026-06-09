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

    [Header("Привязка к костям")]
    [SerializeField] protected CharacterBoneSocket boneSocket;

    [Header("Агент")]
    [SerializeField] protected NavMeshAgent agent;

    [Header("Мотор")]
    [SerializeField] protected HumanoidAIMotor motor;
    [SerializeField] protected HumanoidAIController controller;

    [Header("Статы")]
    public CharacterLevelController levelController;
    public CharacterStatsController statsManager;
    public CharacterStatsModifier statsModifier;

    [Header("Система урона")]
    [SerializeField] protected HumanoidAIDamageController damageController;
    [SerializeField] protected HumanoidAIPushReceiver pushReceiver;
    [SerializeField] protected AiFallController fallController;

    [Header("Очки")]
    [SerializeField] protected PointsEmitter pointsEmitter;

    [Header("Лут")]
    [SerializeField] protected CharacterLootDistributer lootDistributer;
 
    [Header("Визуальные эффекты")]
    [SerializeField] protected CharacterEffectVisualizer visualizer;

    [Header("Жизненый цикл")]
    public HumanoidAiLifecycle lifecycle;

    [Header("UI")]
    [SerializeField] private AiHealthUI aiHealthUI;

    public string uid;
    protected AiRagdollController ragdollController;
    protected EnemyAIAnimatorController animatorController;
    protected HumanoidAgentController agentController;


    public virtual void Init()
    {
        uid = uniqueId.uniqueId;

        AnimatorInit();
        boneSocket.Init(animator);
        AgentInit();
        RagdollInit();
        IKInit();
        MotorInit();
        ControllerInit();
        StatsInit();
        DamageInit();
        DistributerInit();
        LifecycleInit();
        InitUI();
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
        levelController.Init(statsController:statsManager);
        statsModifier.Init(statsManager,visualizer,damageController);
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
        damageController.Init(self: transform, motor: motor,ragdollController: ragdollController, animatorController: animatorController,statsController:statsManager,statsModifier:statsModifier,healthUI:aiHealthUI );
        pushReceiver.Init(damageController: damageController, animatorController: animatorController, ragdollController: ragdollController, self: transform);
        fallController.Init(ragdollController: ragdollController, damagable: damageController, self: transform);
    }

    protected void InitUI()
    {
        aiHealthUI.Init(statsManager);
    }

    private void DistributerInit()
    {

    }

    protected abstract void LifecycleInit();
  


    protected abstract void AnimatorInit();

}
