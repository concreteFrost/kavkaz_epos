using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellProjectile", menuName = ScriptablePaths.PROJECTILE_INSTANCE_PATH + "/SpellProjectile")]
public class SpellProjectileSO : ProjectileSO 
{
    [Tooltip("Анимация, которая проигрывается при касте этого заклинания.")]
    public AnimationInfoSO castAnimation;

    [Tooltip("Стоимость использования способности в выносливости.")]
    public float staminaPenalty = 1f;

    [Tooltip("Требования к характеристикам или уровню для использования заклинания.")]
    public ItemRequirements requirements;

    public override bool IsStackable() => true;
    public override bool CanEmit(int i)
    {
        if (!requirements.CanUse(i))
        {  
            return false;
        }

        return true;    
    }

}
