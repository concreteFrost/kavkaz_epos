using UnityEngine;

public class EnemyServiceLocator : BaseHumanoidAiServiceLocator
{

    [Header("Система взаимодействия")]
    [SerializeField] private ItemCollector interaction;

    [Header("Боевая система")]
    [SerializeField] private BaseHumanoidCombatController combatController;
    [SerializeField] private HumanoidCombatInventory combatInventory;
    [SerializeField] private AttackSource attackSource;

    [Header("Магичкеская система")]
    [SerializeField] private CharacterEmitter emitter;
    [SerializeField] private CharacterSpellInventory spellInventory;

    [Header("Система прерывания состояний")]
    [SerializeField] private InterruptionManager interruptionManager;

    [Header("Система зрения")]
    [SerializeField] private EnemyFOVController fovController;

    [Header("Мозг")]
    [SerializeField] protected EnemyBrain brain;
    [SerializeField] protected EnemyStateTracker stateTracker;

    [Header("Система событий")]
    [SerializeField] private EnemyNotifierManager notifierManager;

    protected override void AnimatorInit()
    {
        animatorController = new HumanoidAIAnimatorController();
        animatorController.Init(animator: animator, overrideController: overrideController, motor: motor, combatController: combatController, targetLock: fovController, damageController: damageController, pushReceiver: pushReceiver);
    }
    protected override void CoreInit()
    {
        base.CoreInit();

        InterruptorsInit();
        InteractionInit();
        FovInit();
        CombatInit();
        SpellInit();
        //notifierManager.Init()
        BrainInit();

    }

    private void FovInit()
    {
        fovController.Init(boneSockets:boneSocket);
    }

    private void InteractionInit()
    {
        interaction.Init(self: transform, animatorController: animatorController, combatInventory: combatInventory, damageController: damageController, attackSource: attackSource);
    }

    private void CombatInit()
    {
        //всегда инициализировать ранььше combatInventory потому что переставив их местами у оружия attack source может быть null
        attackSource.Init(sourcePosition: transform, sourceId: (int)damageController.CharacterType);
        combatController.Init(combatInventory: combatInventory, animatorController: animatorController, damageController: damageController);
        combatInventory.Init(boneSocket:boneSocket,animatorController: animatorController, combatController: combatController, collector: interaction);
    }

    private void SpellInit()
    {
        spellInventory.Init();
        emitter.Init(spellInventory: spellInventory, source: attackSource, animatorController: animatorController, targetLocker: fovController, boneSockets: boneSocket);
       
    }

    private void InterruptorsInit()
    {
        notifierManager.Init(self: transform, fov: fovController);
        interruptionManager.Init(damageController: damageController, pushReceiver: pushReceiver);
    }

    protected override void LifecycleInit()
    {
        lifecycle.Init(damagable: damageController, statsController: statsManager,statsModifier:statsModifier, ragdollController: ragdollController, brain: brain, startingPosition:transform.position, self:transform);
    }


    protected  void BrainInit()
    {

        stateTracker.Init(damageController: damageController, statsController: statsManager);

        EnemyBrainContext brainContext = new EnemyBrainContext()
        {
            permamentPosition = transform.position,
            self = transform,
            animator = animator,
            motor = motor,
            controller = controller,
            stats = statsManager,
            damageController = damageController,
            combat = combatController,
            inventory = combatInventory,
            emitter = emitter,
            spellInventory = spellInventory,
            fov = fovController,
            interact = interaction,
            stateTracker = stateTracker,
            agentController = agentController,
            ragdollController = ragdollController,
            interruptionManager = interruptionManager,
            notifierManager = notifierManager

        };

        brain.Init(brainContext);
    }


}
