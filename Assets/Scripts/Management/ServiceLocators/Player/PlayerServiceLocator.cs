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

    [Header("Привязка к костям")]
    [SerializeField] private CharacterBoneSocket boneSocket;

    [Header("Мотор и перемещение")]
    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerClimbing climbing;

    [Header("Ввод")]
    [SerializeField] private PlayerInput input;

    [Header("Контроллеры")]
    [SerializeField] private PlayerLocomotionActionHandler locomotionHandler;
    [SerializeField] private PlayerCombatActionHandler combatHandler;
    [SerializeField] private PlayerQuickSlotActionHandler quickSlotHandler;

    [Header("Статы")]
    [SerializeField] private CharacterStatsController stats;
    [SerializeField] private CharacterStatsModifier statsModifier;

    [Header("Система взаимодействия")]
    [SerializeField] private ItemCollector interaction;

    [Header("Боевая система")]
    [SerializeField] private BaseHumanoidCombatController combatController;
    [SerializeField] private AttackSource attackSource;
    [SerializeField] private AgressivePushController pushController;

    [Header("Магическая система")]
    [SerializeField] private CharacterEmitter emitterController;
   
    [Header("Инвентари и быстрые слоты")]
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private CharacterSpellInventory spellInventory;


    [Header("Система урона")]
    [SerializeField] private PlayerDamageController damageController;
    [SerializeField] private PlayerPushReceiver pushReceiver;
    [SerializeField] private PlayerFallController fallController;

    [Header("Визуальные эффекты")]
    [SerializeField] private CharacterEffectVisualizer effectVisualizer;

    [Header("Жизненый цикл")]
    [SerializeField] private PlayerLifecycle lifecycle;

    [Header("Система прицеливания")]
    [SerializeField] private PlayerTargetLock targetLock;

    [Header("Прочее")]
    private PlayerActionGuards actionGuards;

    [Header("UI")]
    [SerializeField] private PlayerUIManager uiManager;  


    private void Awake()
    {
        uid = uniqueId.uniqueId;

        boneSocket.Init(animator);

        actionGuards = new PlayerActionGuards(locomotion:motor,stats:stats,damageController:damageController,climbing:climbing, emitter:emitterController,meleeCombat:combatController);

        animatorController.Init(
            animator:animator,
            overrideController:overrideController,
            combatController:combatController,
            motor:motor,targetLock:targetLock,
            damageController:damageController,
            pushReceiver:pushReceiver
            );

        input.Init(locomotion:locomotionHandler,combatHandler:combatHandler ,animatorController: animatorController, targetLock: targetLock,quickSlotHandler:quickSlotHandler, uiManager:uiManager);
       
        damageController.Init(motor: motor, statsController: stats, animatorController: animatorController,statsModifier:statsModifier);
        interaction.Init(self: transform, animatorController: animatorController, combatInventory: combatInventory, damageController: damageController, attackSource: attackSource);
        
        //всегда инициализировать ранььше combatInventory
        attackSource.Init(sourcePosition: this.transform, sourceId: (int)damageController.CharacterType);

        combatController.Init(combatInventory:combatInventory,animatorController:animatorController,damageController:damageController);
        combatInventory.Init(boneSocket:boneSocket,animatorController: animatorController, combatController: combatController, collector: interaction);

        emitterController.Init(spellInventory:spellInventory, source:attackSource,animatorController:animatorController,targetLocker:targetLock, boneSockets:boneSocket);
          
        pushController.Init(attackSource: attackSource, combatController: combatController, animatorController: animatorController, self: transform);
        climbing.Init(motor: motor, actionGuards: actionGuards, animatorController: animatorController);
        fallController.Init(motor: motor, damageController: damageController);
        targetLock.Init(controller:locomotionHandler,damageController:damageController);
        
        stats.Init();
        statsModifier.Init(stats,visualizer:effectVisualizer);
        motor.Init(animatorController: animatorController);

        locomotionHandler.Init(motor: motor, interaction: interaction, actionGuards: actionGuards, stats: stats, climbing: climbing);
        combatHandler.Init(actionGuards: actionGuards,combatController:combatController,pushSource:pushController,emitController:emitterController);
        quickSlotHandler.Init(spellInventory: spellInventory, actionGuards: actionGuards);
       
        lifecycle.Init(damagable:damageController,statsController:stats,statsModifier:statsModifier,input:input, startingPosition: transform.position, self:transform);

        uiManager.Init(stats:stats, spellInventory:spellInventory,combatInventory:combatInventory,targetLock:targetLock, input:input);

    }

   


}
