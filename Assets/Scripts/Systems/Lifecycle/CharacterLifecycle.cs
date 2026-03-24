using UnityEngine;

public abstract class CharacterLifecycle : MonoBehaviour
{

    protected BaseHumanoidDamageController damagable;
    protected CharacterStatsController statsController;
    protected CharacterStatsModifier statsModifier;
    protected Vector3 startingPosition;
    protected Transform self;

    public abstract void Die();
    public abstract void Respawn();

    protected void BaseInit(CharacterStatsController statsController, CharacterStatsModifier statsModifier, BaseHumanoidDamageController damageController, Vector3 startingPostion, Transform self)
    {
        this.statsModifier = statsModifier;
        this.statsController = statsController;
        this.damagable = damageController;
        this.startingPosition = startingPostion;
        this.self = self;

        statsController.Health.Depleted += Die;
    }

    private void OnDisable()
    {
        statsController.Health.Depleted -= Die;
    }

    protected void ResetPosition()
    {
        self.position = startingPosition;
    }

}
