using UnityEngine;

public abstract class CharacterLifecycle : MonoBehaviour
{

    protected IDamagable damagable;
    protected CharacterStatsController statsController;
    protected CharacterStatsModifier statsModifier;

    public abstract void Die();
    public abstract void Respawn();

    protected void BaseInit(CharacterStatsController statsController, CharacterStatsModifier statsModifier, IDamagable damageController)
    {
        this.statsModifier = statsModifier;
        this.statsController = statsController;
        this.damagable = damageController;

        statsController.Health.Depleted += Die;
    }

    private void OnDisable()
    {
        statsController.Health.Depleted -= Die;
    }

}
