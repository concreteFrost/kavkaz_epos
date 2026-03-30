using UnityEngine;

public abstract class CharacterLifecycle : MonoBehaviour , ICharacterLifeCycle
{

    protected BaseHumanoidDamageController damagable;
    protected CharacterStatsController statsController;
    protected CharacterStatsModifier statsModifier;
    public Vector3 respawnPosition;
    protected Transform self;

    public abstract void Die();
    public abstract void Respawn();

    protected void BaseInit(CharacterStatsController statsController, CharacterStatsModifier statsModifier, BaseHumanoidDamageController damageController, Vector3 startingPostion, Transform self)
    {
        this.statsModifier = statsModifier;
        this.statsController = statsController;
        this.damagable = damageController;
        this.respawnPosition = startingPostion;
        this.self = self;

        statsController.Health.Depleted += Die;
    }

    private void OnDisable()
    {
        statsController.Health.Depleted -= Die;
    }
    
    public void SetStartingPosition(Vector3 pos)
    {
        respawnPosition = pos;
    }

    public void ResetPosition()
    {
        self.position = respawnPosition;
    }

}
