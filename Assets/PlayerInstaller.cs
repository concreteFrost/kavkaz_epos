using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{

    [Header("Animator")]
    [SerializeField] private PlayerMotor motor;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController overrideController;


     public override void InstallBindings()
    {
        
        // Core components
        Container.Bind<PlayerMotor>().FromInstance(motor);
        Container.Bind<Animator>().FromInstance(animator);
        Container.Bind<AnimatorOverrideController>().FromInstance(overrideController);


        Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerClimbing>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<CharacterInteract>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerTargetLock>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerFallController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerDamageController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerPushReceiver>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<CharacterStatsController>().FromComponentInHierarchy().AsSingle().NonLazy();

        // Combat
        Container.Bind<BaseHumanoidCombatController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<HumanoidCombatInventory>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<AttackSource>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<AgressivePushController>().FromComponentInHierarchy().AsSingle().NonLazy();
       

       

        // Action guards & Animator
        Container.Bind<PlayerActionGuards>().AsSingle().NonLazy();
        Container.Bind<PlayerAnimatorController>().AsSingle().NonLazy();

        // Transform / IDs
        Container.Bind<Transform>().FromInstance(transform).AsSingle().NonLazy();
        Container.Bind<int>().FromInstance(transform.gameObject.GetInstanceID()).AsSingle().NonLazy();


        // Interfaces
        Container.Bind<IDamagable>().To<PlayerDamageController>().FromResolve();
        Container.Bind<IHumanoidCombat>().To<BaseHumanoidCombatController>().FromResolve();
        Container.Bind<IAttackSource>().To<AttackSource>().FromResolve();
        Container.Bind<ICombatInventory>().To<HumanoidCombatInventory>().FromResolve();
        Container.Bind<ICollector>().To<CharacterInteract>().FromResolve();
        Container.Bind<IHumanoidMovement>().To<PlayerMotor>().FromResolve();
        Container.Bind<IClimber>().To<PlayerClimbing>().FromResolve();
        Container.Bind<IPushSource>().To<AgressivePushController>().FromResolve();
        Container.Bind<IPushable>().To<PlayerPushReceiver>().FromResolve();
        Container.Bind<ITargetLocker>().To<PlayerTargetLock>().FromResolve();

      
        // Base controller
        Container.Bind<BaseHumanoidAnimatorController>().To<PlayerAnimatorController>().FromResolve();

        // Animator / UI
        Container.Bind<LockOnTargetUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerStatsUI>().FromComponentInHierarchy().AsSingle().NonLazy();



    }




}
