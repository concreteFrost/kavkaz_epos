using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.CONSUMABLE_ITEM_PATH + "/Continuous Status Effect Item", fileName = "Continuous Status Effect Item")]
public class ContinuousStatModifierItemSO : ConsumableItemSO
{
    public ContiniousStatusEffectData effectData;
    public void UseItem(CharacterStatsModifier ctx)
    {
        ctx.AddSideEffect(effectData);
    }

}








