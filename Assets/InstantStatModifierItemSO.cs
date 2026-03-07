using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Character Status Effect Item" , fileName ="Character Status Effect Item")]
public class InstantStatModifierItemSO: ConsumableItemSO<CharacterStatsModifier>
{
   
    public override void UseItem(CharacterStatsModifier ctx)
    {
        ctx.ApplyInstantSideEffect(effectData);
    }

}





