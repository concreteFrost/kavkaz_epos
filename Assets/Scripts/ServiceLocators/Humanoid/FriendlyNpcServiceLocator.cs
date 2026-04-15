using UnityEngine;

public class FriendlyNpcServiceLocator : BaseHumanoidAiServiceLocator
{
    [Header("Система взаимодействия")]
    [SerializeField] private HumanoidAiInteractionController interaction;

    [Header("Боевая система")]
    [SerializeField] private BaseHumanoidCombatController combatController;
    [SerializeField] private HumanoidWeaponSetter weaponSetter;
    [SerializeField] private AttackSource attackSource;
    [SerializeField] private CharacterWeaponInventory weaponInventory;

    [Header("Система зрения")]
    public EnemyFOVController fovController;

    [Header("Диалоговая Система")]
    public NpcDialogueController dialogueController;

    public override void Init()
    {
        base.Init();
        
        InteractionInit();
        DialoguesInit();
        CombatInit();
    }

    protected override void AnimatorInit()
    {
        animatorController = new EnemyAIAnimatorController();
        animatorController.Init(animator: animator, overrideController: overrideController, motor: motor, combatController: combatController, targetLock: fovController, damageController: damageController, pushReceiver: pushReceiver);
    }

    protected override void LifecycleInit()
    {
        
    }



    private void DialoguesInit()
    {
        dialogueController.Init(animatorController);
    }
    private void InteractionInit()
    {
        interaction.Init(collectorId: uniqueId.uniqueId, self: transform, statsController: statsManager, statsModifier: statsModifier, animatorController: animatorController, combatInventory: weaponSetter, damageController: damageController, attackSource: attackSource, lifeCycle: lifecycle);
    }


    private void CombatInit()
    {
        //всегда инициализировать ранььше combatInventory потому что переставив их местами у оружия attack source может быть null
        attackSource.Init(sourcePosition: transform, sourceId: (int)damageController.CharacterType);
        combatController.Init(combatInventory: weaponSetter, animatorController: animatorController, damageController: damageController);
        weaponSetter.Init(boneSocket: boneSocket, animatorController: animatorController, combatController: combatController, collector: interaction, enableWeaponBreakdown: false);
        weaponInventory.Init(weaponSetter);
    }
}
