using UnityEngine;

public class HumanoidAIServiceLocator : MonoBehaviour
{
    [SerializeField] UniqueId uniqueId;

    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private HumanoidAIMotor motor;
    [SerializeField] private HumanoidAIController controller;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private CharacterStatsController statsController;
    [SerializeField] private HumanoidAITargetLock targetLock;
    [SerializeField] private HumanoidAIAnimatorController animatorController = new HumanoidAIAnimatorController();
    [SerializeField] private HumanoidAIDamageController damageController;
    [SerializeField] private HumanoidCombatController combatController;
 

    private void Awake()
    {
        string uid = uniqueId.uniqueId;

        stats.Init();
        motor.Init(animator);

        HumanoidAnimatorService animatorService = new HumanoidAnimatorService(animator,motor,combatController,targetLock,damageController);
        animatorController.Init(animatorService);
       
        HumanoidStatsControllerService service = new HumanoidStatsControllerService(stats);
        statsController.Init(service);

        HumanoidControllerService controllerService = new HumanoidControllerService(animator, motor, animatorController, statsController, stats);
        controller.Init(controllerService);

        damageController.Init(statsController, stats,motor.agent,capsuleCollider, uid);

        CharacterTargetLockService targetLockService = new CharacterTargetLockService(controller, damageController, stats);
        targetLock.Init();
    }
}
