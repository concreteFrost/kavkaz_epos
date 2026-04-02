using UnityEngine;

public class EnemyServiceLocator : BaseHumanoidAiServiceLocator
{

    [Header("Система взаимодействия")]
    [SerializeField] private HumanoidAiInteractionController interaction;

    [Header("Боевая система")]
    [SerializeField] private BaseHumanoidCombatController combatController;
    [SerializeField] private HumanoidWeaponSetter weaponSetter;
    [SerializeField] private AttackSource attackSource;
    [SerializeField] private CharacterWeaponInventory weaponInventory;

    [Header("Магичкеская система")]
    [SerializeField] private CharacterEmitter emitter;
    [SerializeField] private CharacterSpellInventory spellInventory;

    [Header("Система прерывания состояний")]
    [SerializeField] private InterruptionManager interruptionManager;

    [Header("Система зрения")]
    [SerializeField] private EnemyFOVController fovController;

    [Header("Мозг")]
    public EnemyBrain brain;
    [SerializeField] protected EnemyStateTracker stateTracker;

    [Header("Система событий")]
    [SerializeField] private EnemyNotifierManager notifierManager;

    protected override void AnimatorInit()
    {
        animatorController = new HumanoidAIAnimatorController();
        animatorController.Init(animator: animator, overrideController: overrideController, motor: motor, combatController: combatController, targetLock: fovController, damageController: damageController, pushReceiver: pushReceiver);
    }
    public override void Init()
    {
        base.Init();

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
        interaction.Init(collectorId:uniqueId.uniqueId, self: transform,statsController:statsManager,statsModifier:statsModifier ,animatorController: animatorController, combatInventory: weaponSetter, damageController: damageController, attackSource: attackSource,lifeCycle:lifecycle);
    }

    private void CombatInit()
    {
        //всегда инициализировать ранььше combatInventory потому что переставив их местами у оружия attack source может быть null
        attackSource.Init(sourcePosition: transform, sourceId: (int)damageController.CharacterType);
        combatController.Init(combatInventory: weaponSetter, animatorController: animatorController, damageController: damageController);
        weaponSetter.Init(boneSocket:boneSocket,animatorController: animatorController, combatController: combatController, collector: interaction, enableWeaponBreakdown:false);
        weaponInventory.Init(weaponSetter);
    }

    private void SpellInit()
    {
        spellInventory.Init();
        spellInventory.SetDefaultQuickSlotData();
        emitter.Init(source: attackSource, animatorController: animatorController, targetLocker: fovController, boneSockets: boneSocket,spellInventory:spellInventory,statsController:statsManager);
       
    }

    private void InterruptorsInit()
    {
        notifierManager.Init(self: transform, fov: fovController);
        interruptionManager.Init(damageController: damageController, pushReceiver: pushReceiver);
    }

    protected override void LifecycleInit()
    {
        lifecycle.Init(damagable: damageController, statsController: statsManager,statsModifier:statsModifier, ragdollController: ragdollController, brain: brain, startingPosition:transform.position,pointsEmitter:pointsEmitter, self:transform, distributer:lootDistributer);
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
            weaponSetter = weaponSetter,
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
