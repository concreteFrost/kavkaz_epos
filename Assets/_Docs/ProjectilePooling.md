# Projectile pooling

ProjectilePoolManager is a child of Prefabs/Core/GameRunner.prefab. It persists with GameRunner. Each prefab owns a separate on-demand pool. During gameplay instances are returned, not destroyed. Pools retain their peak size until the next load/reset.

ProjectileSO.CreateProjectile builds the existing ProjectileData and requests a projectile. Attack SOs, emitters, movement SOs and IProjectile remain unchanged. Projectile prefabs must have Projectile (including subclasses) on their root; debris prefabs require ProjectileDebris on their root.

New objects are instantiated under an inactive storage parent. Preparation and transform placement happen before activation, so movement and physics cannot see the previous shot. DamageCollider supports preparation before its first Awake. Returned projectiles stop coroutines, disable/reset damage, drop source/target data, stop and release owned FMOD events, clear particles/trails, and restore the destructive projectile renderer.

Debris caches original child local positions/rotations and Rigidbody components once. Preparation restores the fragments while inactive. Return makes bodies kinematic, clears velocity and effects, and stops the cleanup coroutine; activation starts the next scatter.

Loading is controlled through explicit calls, without event subscriptions:
- SceneTransitionManager.TransitionToScene calls BeginLoading before TransitionStarted. This immediately clears active/inactive instances and prefab dictionaries, and blocks spawning during the transition.
- After the scene is loaded it calls EndLoading before bootstrap callbacks.
- LevelManager clears pools before LoadLevelState, ReloadWholeLevelState, and ReloadLevelOnRest.
- Future scene/load entry points must use the same lifecycle calls.

ClearPools disables and resets objects immediately and uses Unity Destroy (actual destruction is deferred to the end of the frame). It releases the manager's asset references; asset unloading remains Unity's responsibility. No Resources.UnloadUnusedAssets is forced during combat or rest.

Verification:
- All current Assembly-CSharp sources, including the manager, compile against this project's Unity and plugin assemblies with C# 9. No errors; four existing unrelated warnings.
- Play Mode checks still required: reuse after hit/timeout, destructive mesh visibility, fragment reconstruction and motion, repeated burst/rain attacks, FMOD hit/lifetime cleanup, no residual particles/trails/damage, travel, save loading, new game/menu and repeated bonfire resets.
- In the hierarchy, active and inactive instances should be reused during a level. After loading/reset no old pooled children should remain after the destruction frame, and the next attack should create its new pool.
