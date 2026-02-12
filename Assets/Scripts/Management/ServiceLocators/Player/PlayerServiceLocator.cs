using UnityEngine;

public class PlayerServiceLocator : MonoBehaviour
{
    [Header("Уникальный идентификатор")]
    [SerializeField] private UniqueId uniqueId;
    private string uid;

    [Header("Анимация")]
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController overrideController;
    [SerializeField] private PlayerAnimatorController animatorController = new PlayerAnimatorController();

    [Header("Мотор и перемещение")]
    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerClimbing climbing;

    [Header("Ввод")]
    [SerializeField] private PlayerInput input;

    [Header("Контроллер")]
    [SerializeField] private PlayerController controller;

    [Header("Статы")]
    [SerializeField] private CharacterStatsController stats;

    [Header("Система взаимодействия")]
    [SerializeField] private ItemCollector interaction;

    [Header("Боевая система")]
    [SerializeField] private BaseHumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private AttackSource attackSource;
    [SerializeField] private AgressivePushController pushController;

    [Header("Система урона")]
    [SerializeField] private PlayerDamageController damageController;
    [SerializeField] private PlayerPushReceiver pushReceiver;
    [SerializeField] private PlayerFallController fallController;

    [Header("Система прицеливания")]
    [SerializeField] private PlayerTargetLock targetLock;

    [Header("UI")]
    [SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private LockOnTargetUI lockOnTargetUI;

    [Header("Прочее")]
    private PlayerActionGuards actionGuards;    

    private void Awake()
    {
        uid = uniqueId.uniqueId;

        actionGuards = new PlayerActionGuards(locomotion:motor,stats:stats,damageController:damageController,climbing:climbing);

        animatorController.Init(
            animator:animator,
            overrideController:overrideController,
            combatController:combatController,
            motor:motor,targetLock:targetLock,
            damageController:damageController,
            pushReceiver:pushReceiver
            );

     

        input.Init(controller: controller, animatorController: animatorController, targetLock: targetLock);
       
        damageController.Init(motor: motor, stats: stats, animatorController: animatorController);
        interaction.Init(self: transform, animatorController: animatorController, combatInventory: combatInventory, damageController: damageController, attackSource: attackSource);
        
        //всегда инициализировать ранььше combatInventory
        attackSource.Init(sourcePosition: this.transform, sourceId: (int)damageController.CharacterType);

        combatController.Init(combatInventory:combatInventory,animatorController:animatorController,damageController:damageController);
        combatInventory.Init(animatorController: animatorController, combatController: combatController, collector: interaction);
          
        pushController.Init(attackSource: attackSource, combatController: combatController, animatorController: animatorController, self: transform);
        climbing.Init(motor: motor, actionGuards: actionGuards, animatorController: animatorController);
        fallController.Init(motor: motor, damageController: damageController);
        targetLock.Init(lockOnTargetUI:lockOnTargetUI,controller:controller,damageController:damageController);
        
        stats.Init();
        motor.Init(animatorController: animatorController);
        controller.Init(motor: motor, combatController: combatController, interaction: interaction, actionGuards: actionGuards, stats: stats, pushSource: pushController, climbing: climbing);
       

        playerStatsUI.Init(stats: stats);

    }

   


}
