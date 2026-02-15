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

    [Header("Контроллеры")]
    [SerializeField] private PlayerLocomotionActionHandler locomotionHandler;
    [SerializeField] private PlayerCombatActionHandler combatHandler;

    [Header("Статы")]
    [SerializeField] private CharacterStatsController stats;

    [Header("Система взаимодействия")]
    [SerializeField] private ItemCollector interaction;

    [Header("Боевая система")]
    [SerializeField] private BaseHumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private AttackSource attackSource;
    [SerializeField] private AgressivePushController pushController;

    [Header("Магическая система")]
    [SerializeField] private CharacterEmitter emitterController;
    [SerializeField] private CharacterSpellInventory spellInventory;

    [Header("Система урона")]
    [SerializeField] private PlayerDamageController damageController;
    [SerializeField] private PlayerPushReceiver pushReceiver;
    [SerializeField] private PlayerFallController fallController;

    [Header("Жизненый цикл")]
    [SerializeField] private PlayerLifecycle lifecycle;

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

        actionGuards = new PlayerActionGuards(locomotion:motor,stats:stats,damageController:damageController,climbing:climbing, emitter:emitterController,meleeCombat:combatController);

        animatorController.Init(
            animator:animator,
            overrideController:overrideController,
            combatController:combatController,
            motor:motor,targetLock:targetLock,
            damageController:damageController,
            pushReceiver:pushReceiver
            );

     

        input.Init(controller: locomotionHandler,combatHandlder:combatHandler ,animatorController: animatorController, targetLock: targetLock);
       
        damageController.Init(motor: motor, stats: stats, animatorController: animatorController);
        interaction.Init(self: transform, animatorController: animatorController, combatInventory: combatInventory, damageController: damageController, attackSource: attackSource);
        
        //всегда инициализировать ранььше combatInventory
        attackSource.Init(sourcePosition: this.transform, sourceId: (int)damageController.CharacterType);

        combatController.Init(combatInventory:combatInventory,animatorController:animatorController,damageController:damageController);
        combatInventory.Init(animatorController: animatorController, combatController: combatController, collector: interaction);

        emitterController.Init(spellInventory:spellInventory, source:attackSource,animatorController:animatorController,targetLocker:targetLock);
          
        pushController.Init(attackSource: attackSource, combatController: combatController, animatorController: animatorController, self: transform);
        climbing.Init(motor: motor, actionGuards: actionGuards, animatorController: animatorController);
        fallController.Init(motor: motor, damageController: damageController);
        targetLock.Init(lockOnTargetUI:lockOnTargetUI,controller:locomotionHandler,damageController:damageController);
        
        stats.Init();
        motor.Init(animatorController: animatorController);

        locomotionHandler.Init(motor: motor, interaction: interaction, actionGuards: actionGuards, stats: stats, climbing: climbing);
        combatHandler.Init(actionGuards: actionGuards,combatController:combatController,pushSource:pushController,emitController:emitterController);
       
        lifecycle.Init(damagable:damageController,statsController:stats,input:input); 

        playerStatsUI.Init(stats: stats);

    }

   


}
