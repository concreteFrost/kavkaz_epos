using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Stat Modifier Item", fileName = "Stat Modifier Item")]
public class StatModifierItemSO : ConsumableItemSO
{
    public StatusEffectData effectData;
    public void UseItem(CharacterStatsModifier ctx)
    {
        if (effectData == null) return;
        ctx.GetAndApplyStatusEffect(effectData);
    }


}
