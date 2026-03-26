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
    [SerializeField] private PlayerInputReader inputReader = new PlayerInputReader();
    [SerializeField] private PlayerInputManager inputManager;
    [SerializeField] private PlayerGameInput input;
    [SerializeField] private PlayerUIInput inputUI;

    [Header("Контроллеры")]
    [SerializeField] private PlayerLocomotionActionHandler locomotionHandler;
    [SerializeField] private PlayerCombatActionHandler combatHandler;
    [SerializeField] private PlayerQuickSlotActionHandler quickSlotHandler;

    [Header("Статы")]
    [SerializeField] public CharacterLevelController levelController; 
    [SerializeField] public CharacterStatsController stats;
    [SerializeField] public CharacterStatsModifier statsModifier;

    [Header("Система очков")]
    [SerializeField] private PlayerPointsCollector pointsCollector; 

    [Header("Система взаимодействия")]
    [SerializeField] private PlayerItemsCollector interaction;
    [SerializeField] private CharacterConsumeController consumeController;
    [SerializeField] private PlayerInvalidLootCollector invalidLootCollector;

    [Header("Боевая система")]
    [SerializeField] private BaseHumanoidCombatController combatController;
    [SerializeField] private AttackSource attackSource;
    [SerializeField] private AgressivePushController pushController;

    [Header("Магическая система")]
    [SerializeField] private CharacterEmitter emitterController;

    [Header("Инвентари и быстрые слоты")]
    [SerializeField] public HumanoidCombatInventory combatInventory;
    [SerializeField] public CharacterSpellInventory spellInventory;
    [SerializeField] public PlayerConsumableInventory consumableInventory;  

    [Header("Система урона")]
    [SerializeField] private PlayerDamageController damageController;
    [SerializeField] private PlayerPushReceiver pushReceiver;
    [SerializeField] private PlayerFallController fallController;

    [Header("Визуальные эффекты")]
    [SerializeField] private CharacterEffectVisualizer effectVisualizer;

    [Header("Жизненный цикл")]
    [SerializeField] private PlayerLifecycle lifecycle;

    [Header("Конструктор персонажа")]
    [SerializeField] private CharacterConstructor constructor;

    [Header("Система прицеливания")]
    [SerializeField] private PlayerTargetLock targetLock;

    [Header("UI")]
    [SerializeField] private PlayerUIManager uiManager;

    private PlayerActionGuards actionGuards;

    public void Init()
    {
        InitCore();
        InitInput();
        InitAnimation();
        InitInteraction();
        InitCombat();
        InitMovement();
        InitStats();
        InitPoints();
        InitInventories();
      
        InitLifecycle();
        InitCharacterConstructor(); 
        InitUI();
    }

    private void InitCore()
    {
        uid = uniqueId.uniqueId;
        boneSocket.Init(animator);

        actionGuards = new PlayerActionGuards(
            locomotion: motor,
            stats: stats,
            damageController: damageController,
            climbing: climbing,
            emitter: emitterController,
            meleeCombat: combatController,
            consumeController:consumeController);
    }

    private void InitInput()
    {
        inputReader.Init();
        inputManager.Init(reader: inputReader);

        input.Init(
            reader: inputReader,
            locomotion: locomotionHandler,
            combatHandler: combatHandler,
            animatorController: animatorController,
            targetLock: targetLock,
            quickSlotHandler: quickSlotHandler
            );

        inputUI.Init(reader: inputReader);
    }

    private void InitAnimation()
    {
        animatorController.Init(
            animator: animator,
            overrideController: overrideController,
            combatController: combatController,
            motor: motor,
            targetLock: targetLock,
            damageController: damageController,
            pushReceiver: pushReceiver);
    }

    private void InitCombat()
    {
        attackSource.Init(
            sourcePosition: transform,
            sourceId: (int)damageController.CharacterType);

        combatController.Init(
            combatInventory: combatInventory,
            animatorController: animatorController,
            damageController: damageController);

        emitterController.Init(
            spellInventory: spellInventory,
            source: attackSource,
            animatorController: animatorController,
            targetLocker: targetLock,
            boneSockets: boneSocket,
            statsController:stats
            );

        pushController.Init(
            attackSource: attackSource,
            combatController: combatController,
            animatorController: animatorController,
            self: transform);


      

        combatHandler.Init(
            actionGuards: actionGuards,
            combatController: combatController,
            pushSource: pushController,
            emitController: emitterController);

        quickSlotHandler.Init(
            spellInventory: spellInventory,
            consumableInventory: consumableInventory,   
            actionGuards: actionGuards);
    }

    private void InitInteraction()
    {

        interaction.Init(
            collectorId:uid,
            self: transform,
            statsController: stats,
            animatorController: animatorController,
            combatInventory: combatInventory,
            damageController: damageController,
            attackSource: attackSource,
            spellInventory: spellInventory,
            consumableInventory: consumableInventory);

        consumeController.Init(animatorController: animatorController, inventory: consumableInventory);

        invalidLootCollector.Init(collector: interaction);

    }

    private void InitMovement()
    {
        motor.Init(animatorController: animatorController);

        climbing.Init(
            motor: motor,
            actionGuards: actionGuards,
            animatorController: animatorController);

        fallController.Init(
            motor: motor,
            damageController: damageController, self:transform);

        targetLock.Init(
            controller: locomotionHandler,
            damageController: damageController);

        locomotionHandler.Init(
            motor: motor,
            interaction: interaction,
            actionGuards: actionGuards,
            stats: stats,
            climbing: climbing,
            consumeController:consumeController);
    }

    private void InitStats()
    {
        stats.Init();
        levelController.Init(statsController: stats);
        statsModifier.Init(stats, visualizer: effectVisualizer,damageController);

        damageController.Init(
            motor: motor,
            statsController: stats,
            animatorController: animatorController,
            statsModifier: statsModifier);
    }

    private void InitPoints()
    {
        pointsCollector.Init(levelController: levelController);
    }

    private void InitInventories()
    {
        combatInventory.Init(
            boneSocket: boneSocket,
            animatorController: animatorController,
            combatController: combatController,
            collector: interaction,
            enableWeaponBreakdown:true);

        spellInventory.Init();
        spellInventory.SetDefaultQuickSlotData();
        consumableInventory.Init(combatInventory:combatInventory,statsModifier:statsModifier,pointsCollector:pointsCollector);
    }

    private void InitLifecycle()
    {
        lifecycle.Init(
            damagable: damageController,
            statsController: stats,
            statsModifier: statsModifier,
            startingPosition: transform.position,
            self: transform);
    }

    private void InitUI()
    {
        uiManager.Init(
            stats: stats,
            statsModifier:statsModifier,
            spellInventory: spellInventory,
            consumableInventory: consumableInventory,   
            combatInventory: combatInventory,
            targetLock: targetLock,levelController:levelController,
            consumeController:consumeController);
    }

    private void InitCharacterConstructor()
    {
        constructor.Init(consumableInventory: consumableInventory);
    }
}