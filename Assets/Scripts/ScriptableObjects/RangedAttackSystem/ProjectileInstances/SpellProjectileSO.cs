using UnityEngine;

[CreateAssetMenu(fileName = "SpellProjectile", menuName = ScriptablePaths.PROJECTILE_INSTANCE_PATH + "/SpellProjectile")]
public class SpellProjectileSO : ProjectileSO 
{
    public AnimationInfoSO animation;
    public float staminaPenalty = 1f;

    public ItemRequirements Requirements;

    public override bool CanEmit(IStatModel model)
    {
        if (!Requirements.CanUse(model))
        {
            Debug.Log($"your current level is {model.CurrentLevel()} and this spell requires {Requirements.minRequired} ");
            return false;
        }

        return true;    
    }
}
