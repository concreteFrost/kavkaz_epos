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
    [SerializeField] private CharacterTargetLock targetLock;
    [SerializeField] private HumanoidAIAnimator animatorController = new HumanoidAIAnimator();
    [SerializeField] private HumanoidAIDamageController damageController;


    private void Awake()
    {
        string uid = uniqueId.uniqueId;

        animatorController.Init(animator, motor, targetLock, damageController);
        stats.Init();

        HumanoidStatsControllerService service = new HumanoidStatsControllerService(stats);
        statsController.Init(service);

        motor.Init(animator);
        controller.Init(motor, animator, animatorController, stats, statsController);

        damageController.Init(statsController, stats,motor.agent,capsuleCollider, uid);

        CharacterTargetLockService targetLockService = new CharacterTargetLockService(controller, damageController);
        targetLock.Init(targetLockService);
    }
}
