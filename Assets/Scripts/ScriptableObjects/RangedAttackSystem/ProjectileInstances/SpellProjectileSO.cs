using UnityEngine;

[CreateAssetMenu(fileName = "SpellProjectile", menuName = ScriptablePaths.PROJECTILE_INSTANCE_PATH + "/SpellProjectile")]
public class SpellProjectileSO : ProjectileSO 
{
    public AnimationInfoSO animation;
    public float staminaPenalty = 1f;

    public ItemRequirements Requirements;

    public override bool CanEmit(int i)
    {
        if (!Requirements.CanUse(i))
        {  
            return false;
        }

        return true;    
    }
}
